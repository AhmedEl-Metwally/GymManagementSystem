(function () {
    'use strict';

    const root = document.getElementById('assistant-widget');
    if (!root) return;

    const askUrl = root.dataset.askUrl;
    const historyUrl = root.dataset.historyUrl;
    const clearUrl = root.dataset.clearUrl;
    const token = root.querySelector('input[name="__RequestVerificationToken"]').value;

    const toggleBtn = document.getElementById('aw-toggle');
    const closeBtn = document.getElementById('aw-close');
    const clearBtn = document.getElementById('aw-clear');
    const windowEl = document.getElementById('aw-window');
    const messagesEl = document.getElementById('aw-messages');
    const form = document.getElementById('aw-form');
    const input = document.getElementById('aw-input');
    const sendBtn = document.getElementById('aw-send');

    const REQUEST_TIMEOUT_MS = 180000;
    const GREETING = 'أهلاً  اسألني عن الباقات أو الجلسات أو الأعضاء (حسب صلاحياتك).';

    let historyLoaded = false;
    let busy = false;

    // ---------- Rendering (never innerHTML with model output) ----------
    function renderFormatted(container, text) {
        text.split('\n').forEach((line, index) => {
            if (index > 0) container.appendChild(document.createElement('br'));

            const clean = line.replace(/^\s*[-*]\s+/, '• ');
            clean.split(/(\*\*[^*]+\*\*)/g).forEach(part => {
                if (part.length > 4 && part.startsWith('**') && part.endsWith('**')) {
                    const strong = document.createElement('strong');
                    strong.textContent = part.slice(2, -2);
                    container.appendChild(strong);
                } else if (part) {
                    container.appendChild(document.createTextNode(part));
                }
            });
        });
    }

    function addMessage(role, text) {
        const el = document.createElement('div');
        el.className = 'aw-msg ' + role;
        el.setAttribute('dir', 'auto');

        if (role === 'bot') renderFormatted(el, text);
        else el.textContent = text;

        messagesEl.appendChild(el);
        scrollToBottom();
        return el;
    }

    function scrollToBottom() {
        messagesEl.scrollTop = messagesEl.scrollHeight;
    }

    function showTyping() {
        const el = document.createElement('div');
        el.className = 'aw-msg bot';
        el.innerHTML = '<span class="aw-dots"><i></i><i></i><i></i></span> <small class="aw-status"></small>';
        messagesEl.appendChild(el);
        scrollToBottom();

        const status = el.querySelector('.aw-status');
        const timer = setTimeout(() => {
            status.textContent = 'لسه بيفكر... ممكن ياخد لحد دقيقة';
        }, 12000);

        return { remove() { clearTimeout(timer); el.remove(); } };
    }

    function setBusy(value) {
        busy = value;
        input.disabled = value;
        sendBtn.disabled = value;
    }

    function autoResize() {
        input.style.height = 'auto';
        input.style.height = Math.min(input.scrollHeight, 120) + 'px';
    }

    // ---------- Network ----------
    async function post(url, body) {
        const controller = new AbortController();
        const timer = setTimeout(() => controller.abort(), REQUEST_TIMEOUT_MS);
        try {
            return await fetch(url, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': token },
                body: JSON.stringify(body ?? {}),
                signal: controller.signal
            });
        } finally {
            clearTimeout(timer);
        }
    }

    function friendlyError(err) {
        if (err && err.fromServer) return err.message;
        if (err && err.name === 'AbortError') return 'الرد اتأخر أوي. جرّب تاني بعد شوية.';
        if (err && err.message === 'session') return 'انتهت الجلسة. سجّل الدخول تاني وجرّب.';
        if (err && err.status === 400) return 'الصفحة قديمة. حدّث الصفحة (F5) وجرّب تاني.';
        return 'حصلت مشكلة وأنا بجاوب. جرّب تاني.';
    }

    async function loadHistory() {
        historyLoaded = true;
        try {
            const response = await fetch(historyUrl, { headers: { 'Accept': 'application/json' } });
            if (!response.ok || response.redirected) throw new Error('history');

            const turns = await response.json();
            if (turns.length === 0) {
                addMessage('bot', GREETING);
                return;
            }
            turns.forEach(t => addMessage(t.role === 'user' ? 'user' : 'bot', t.content));
        } catch {
            addMessage('bot', GREETING);
        }
    }

    // ---------- Events ----------
    function openWindow() {
        windowEl.hidden = false;
        if (!historyLoaded) loadHistory();
        input.focus();
    }

    toggleBtn.addEventListener('click', () => (windowEl.hidden ? openWindow() : (windowEl.hidden = true)));
    closeBtn.addEventListener('click', () => (windowEl.hidden = true));

    clearBtn.addEventListener('click', async () => {
        if (busy) return;
        try { await post(clearUrl); } catch { /* ignore */ }
        messagesEl.replaceChildren();
        addMessage('bot', GREETING);
    });

    input.addEventListener('input', autoResize);
    input.addEventListener('keydown', e => {
        if (e.key === 'Enter' && !e.shiftKey && !e.isComposing) {
            e.preventDefault();
            form.requestSubmit();
        }
    });

    form.addEventListener('submit', async e => {
        e.preventDefault();
        const text = input.value.trim();
        if (!text || busy) return;

        addMessage('user', text);
        input.value = '';
        autoResize();
        setBusy(true);
        const typing = showTyping();

        try {
            const response = await post(askUrl, { message: text });
            if (response.redirected) throw new Error('session');

            const isJson = (response.headers.get('content-type') || '').includes('application/json');
            const data = isJson ? await response.json() : null;

            if (!response.ok) {
                const serverError = new Error(data?.error || 'error');
                serverError.fromServer = Boolean(data?.error);
                serverError.status = response.status;
                throw serverError;
            }

            if (!data || !data.reply) throw new Error('empty');
            addMessage('bot', data.reply);
        } catch (err) {
            addMessage('error', friendlyError(err));
        } finally {
            typing.remove();
            setBusy(false);
            input.focus();
        }
    });
})();
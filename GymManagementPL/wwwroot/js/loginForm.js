$(function () {
    $('#floatingEmail').focus();

    $('#loginForm').on('submit', function () {
        if ($(this).valid()) {
            const btn = document.getElementById('loginBtn');
            btn.classList.add('loading');
            btn.disabled = true;
        }
    });
});

document.getElementById('photoInput')?.addEventListener('change', function(e) {
    const file = (e.target as HTMLInputElement).files?.[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function(ev) {
            const preview = document.getElementById('photoPreview') as HTMLImageElement | null;
            if (preview) {
                preview.src = ev.target?.result as string;
                preview.style.display = 'block';
            }
        };
        reader.readAsDataURL(file);
    }
});

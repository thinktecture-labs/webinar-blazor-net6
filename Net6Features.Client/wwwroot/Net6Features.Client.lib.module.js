﻿export function beforeStart(options, extensions) {
    var element = document.getElementById('app');
    console.log(element);
    if (!localStorage.getItem('application-theme')) {
        if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            localStorage.setItem('application-theme', 'dark');
        } else {
            localStorage.setItem('application-theme', 'light');
        }
    }
}

export function afterStarted(blazor) {
    console.log("afterStarted");
}
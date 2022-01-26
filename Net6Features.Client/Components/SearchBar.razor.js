export function onDebounceInput(elem, component, interval) {
    elem.addEventListener('input', debounce(e => {
        component.invokeMethodAsync('HandleOnInput', e.target.value);
    }, interval));
}

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}
const darkModeSwitch = document.querySelectorAll(".dark-mode-switch");

const handleDarkModeCLick = (item) => {
    if (item.checked) {
        localStorage.setItem("dark_mode", "dark");
    } else {
        localStorage.setItem("dark_mode", "");
    }
    const localStorageData = localStorage.getItem("dark_mode");
    const handleDarkMode = () => {
        document.body.classList.add("dark");
        item.checked = true;
    }
    const handleLightMode = () => {
        document.body.classList.remove("dark");
    }
    if (localStorageData === "dark") {
        handleDarkMode();
    } else {
        handleLightMode();
    }
}

darkModeSwitch.forEach(item => {
    item.addEventListener("click", function () {
        handleDarkModeCLick(item);
    });
})

window.addEventListener("load", function () {
    const localStorageDarkMode = localStorage.getItem("dark_mode");
    if (localStorageDarkMode === "dark") {
        darkModeSwitch.forEach(item => {
            item.checked = true;
        })
    }
});


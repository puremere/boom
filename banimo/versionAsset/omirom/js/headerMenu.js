const csrfToken2 = document.querySelector('meta[name="csrf-token"]').getAttribute('content');
const productsDropDown = document.querySelector("#products-header");
const dropDownBox = document.querySelector("#dropdown-box-header");
const bgModalWrapper = document.querySelector("#bg-modal-wrapper-menu");
const downIconHeader = document.querySelector(".down-icon-header");
const downIconLogIn = document.querySelector(".down-icon-log-in");
const iconSearchHeader = document.querySelector("#search-icon-header");
const headerWrapperMenu = document.querySelector("#header-wrapper-menu");
const searchBoxToggle = document.querySelector("#search-box-toggle-menu");
const closeSearchHeader = document.querySelector("#close-search-header");
const resultSearch = document.querySelector(".result-search-wrapper-header");
const headerResponsiveMenu = document.querySelector("#header-responsive-menu");
const buttonBurgerMenu = document.querySelector("#button-burger-menu");
const closeBurgerMenu = document.querySelector("#close-burger-menu");
const productLinkResponsive = document.querySelector("#product-link-responsive");
const childHeaderDropDown = document.querySelector("#child-header-nav-responsive");
const closedropDownLinkResponsive = document.querySelector("#close-dropdown-link-responsive");
const toggleLogIn = document.getElementById("toggle-log-in");
const logInButton = document.querySelectorAll(".log-in-btn-toggle");
const profileButton = document.querySelector(".profile-btn-toggle");
const logInButtonResponsive = document.querySelectorAll(".log-in-btn-toggle-responsive");
const profileButtonResponsive = document.querySelector(".profile-btn-toggle-responsive");
const closeLogIn = document.getElementById("close-log-in");
const profilePage = document.getElementById("profile-login-page");
const logoutBtn = document.getElementById("logout-btn");
const inputPhoneLogin = document.getElementById("input-phone-login-page");

/*open bg modal function*/
const openBgModal = () => {
    bgModalWrapper.classList.add("animate__fadeIn");
    bgModalWrapper.classList.remove("animate__fadeOut");
    bgModalWrapper.style.display = "block";
}

/*close bg modal function*/
const closeBgModal = (timeOut) => {
    bgModalWrapper.classList.remove("animate__fadeIn");
    bgModalWrapper.classList.add("animate__fadeOut");
    setTimeout(function () {
        bgModalWrapper.style.display = "none";
    }, timeOut);
}

/*close mega menu function*/
const closeMegaMenu = (timeOut) => {
    dropDownBox.classList.remove("animate__fadeIn");
    dropDownBox.classList.add("animate__fadeOut");
    downIconHeader.classList.remove("rotate-down-icon-header");
    setTimeout(function () {
        dropDownBox.style.display = "none";
    }, timeOut);
    closeBgModal(300);
}

/*open mega menu function*/
const openMegaMenu = () => {
    toggleLogIn.classList.remove("animate__fadeIn_login");
    toggleLogIn.classList.add("animate__fadeOut-login");
    downIconLogIn.classList.remove("rotate-down-icon-log-in");
    dropDownBox.classList.add("animate__fadeIn");
    dropDownBox.style.display = "block";
    dropDownBox.classList.remove("animate__fadeOut");
    openBgModal();
    downIconHeader.classList.add("rotate-down-icon-header");
}

/*open search box function*/
const openSearchBox = (timeOut) => {
    searchBoxToggle.classList.remove("animate__fadeOut");
    searchBoxToggle.style.display = "block";
    searchBoxToggle.classList.add("animate__fadeIn");
    headerWrapperMenu.classList.add("animate__fadeOut");
    headerWrapperMenu.style.display = "none";
    setTimeout(function () {
        searchBoxToggle.style.display = "block";
        headerWrapperMenu.style.display = "none";
    }, timeOut);
}

/*close search box function*/
const closeSearchBox = (timeOut) => {
    searchBoxToggle.classList.add("animate__fadeOut");
    searchBoxToggle.classList.remove("animate__fadeIn");
    headerWrapperMenu.classList.remove("animate__fadeOut");
    setTimeout(function () {
        searchBoxToggle.style.display = "none";
        headerWrapperMenu.style.display = "flex";
    }, timeOut);
}


/*toggle open and close mega menu*/
const dropDownMenu = () => {
    if (dropDownBox.classList.contains("animate__fadeOut")) {
        openMegaMenu();
    } else {
        closeMegaMenu(300);
    }
}

/*close bg modal*/
const closeMenuBg = () => {
    closeMegaMenu(300);
    closeSearchBox(300);
    resultSearch.style.display = "none";
}


/*toggle open and close searchBox header*/
const searchBoxHeader = () => {
    if (searchBoxToggle.classList.contains("animate__fadeOut")) {
        dropDownBox.classList.remove("animate__fadeIn");
        toggleLogIn.classList.remove("animate__fadeIn_login");
        toggleLogIn.classList.add("animate__fadeOut_login");
        downIconLogIn.classList.remove("rotate-down-icon-log-in");
        dropDownBox.classList.add("animate__fadeOut");
        downIconHeader.classList.remove("rotate-down-icon-header");
        dropDownBox.style.display = "none";
        openSearchBox(400);
        openBgModal();
        resultSearch.style.display = "block";
    } else {
        closeSearchBox(200);
        resultSearch.style.display = "none";
        closeBgModal(200);
    }
}


/*burger menu*/
const burgerMenu = () => {
    headerResponsiveMenu.classList.toggle("toggle-burger-menu");
    document.body.style.overflowY = "hidden";
}
const closeBurger = () => {
    headerResponsiveMenu.classList.remove("toggle-burger-menu");
    document.body.style.overflowY = "auto";
}

/*dropDown responsive child*/
const dropDownResponsiveChild = () => {
    childHeaderDropDown.classList.toggle("active-responsive-child-nav");
    closedropDownLinkResponsive.classList.toggle("action");
}


/*close logIn function*/
const closeToggleLogIn = (timeOut) => {
    toggleLogIn.classList.remove("animate__fadeIn_login");
    toggleLogIn.classList.add("animate__fadeOut_login");
    downIconLogIn.classList.remove("rotate-down-icon-log-in");
    closeBgModal(300);
}

/*open logIn function*/
const openToggleLogIn = () => {
    dropDownBox.classList.remove("animate__fadeIn");
    dropDownBox.classList.add("animate__fadeOut");
    downIconHeader.classList.remove("rotate-down-icon-header");
    dropDownBox.style.display = "none";
    toggleLogIn.classList.add("animate__fadeIn_login");
    toggleLogIn.classList.remove("animate__fadeOut_login");
    openBgModal();
    downIconLogIn.classList.add("rotate-down-icon-log-in");
    if(inputPhoneLogin != null){
        inputPhoneLogin.focus();
    }
}

const dropToggleLogIn = () => {
    if (toggleLogIn.classList.contains("animate__fadeOut_login")) {
        openToggleLogIn();
    } else {
        closeToggleLogIn(200);
    }
}

/*open logIn function*/
const openToggleProfile = () => {
    dropDownBox.classList.remove("animate__fadeIn");
    dropDownBox.classList.add("animate__fadeOut");
    downIconHeader.classList.remove("rotate-down-icon-header");
    dropDownBox.style.display = "none";
    toggleLogIn.classList.add("animate__fadeIn_login");
    toggleLogIn.classList.remove("animate__fadeOut_login");
    openBgModal();
    //downIconLogIn.classList.add("rotate-down-icon-log-in");

    profilePage.classList.add('add-page')
}

const dropToggleProfile = () => {
    if (toggleLogIn.classList.contains("animate__fadeOut_login")) {
        openToggleProfile();
    } else {
        closeToggleLogIn(200);
    }
}

const logout = (e) => {
    e.preventDefault();
    $.ajax({
        url: baseURL + "logout",
        method: 'post',
        headers: {
            'X-CSRF-TOKEN': csrfToken2,
            'accept': 'application/json',
        },
        data: {},
        error: (xhr, status, error) => {
            if (xhr.status === 422) {

            }
        },
        success: (result, status, xhr) => {
            location.reload();
        }
    });
}

productsDropDown.addEventListener("click", dropDownMenu);
bgModalWrapper.addEventListener("click", closeMenuBg);
bgModalWrapper.addEventListener("click", closeToggleLogIn);
buttonBurgerMenu.addEventListener("click", burgerMenu);
closeBurgerMenu.addEventListener("click", closeBurger);
iconSearchHeader.addEventListener("click", searchBoxHeader);
closeSearchHeader.addEventListener("click", searchBoxHeader);
productLinkResponsive.addEventListener("click", dropDownResponsiveChild);
closeLogIn.addEventListener('click', dropToggleLogIn);
if (logInButton) {
    logInButton.forEach(item => {
        item.addEventListener("click", dropToggleLogIn);
    });
}

if (logInButtonResponsive) {
    logInButtonResponsive.forEach(item => {
        item.addEventListener("click", dropToggleLogIn);
    });
}
profileButtonResponsive != null ? profileButtonResponsive.addEventListener("click", dropToggleProfile) : null;
profileButton != null ? profileButton.addEventListener("click", dropToggleProfile) : null;
logoutBtn != null ? logoutBtn.addEventListener("click", logout) : null;

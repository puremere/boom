const navbarMenuScroll = document.querySelector(".navbar-menu-scroll");
const productNameFixed = document.querySelector(".product-name-fixed");
const fixedMenuTopResponsive = document.querySelector(".nav-menu-scroll-responsive");
const openSellersResponsive = document.getElementById("open-sellers-responsive");
const sellerToggleResponsive = document.querySelector(".seller-responsive-wrapper");
const backSellerResponsive = document.querySelector(".back-seller-responsive");
const buttonMoreTextStudyProduct = document.querySelector(".more-text-study-product");
const studyProductToggleResponsive = document.querySelector(".study-product-responsive-wrapper");
const backStudyProductResponsive = document.querySelector('.back-study-responsive');
const shareButton = document.querySelectorAll(".share-button");
const bgModalDesktop = document.querySelector(".bg-modal-desktop");
const shareWrapperModal = document.querySelectorAll(".share-wrapper-modal");
const buttonCloseShareModal = document.getElementById("button-close-share-modal");
const navBarSinglePage = document.getElementById("nav-bar-single-page");
const scrollMenuSmooth = document.querySelectorAll(".scroll-menu-smooth");
const textPagePosition = document.getElementById("text-page-position-responsive");
const addFavoriteButton = document.querySelectorAll(".add-favorite");
const copyProductUrl = document.getElementById("copy-product-url");
const starRatingWrapper = document.querySelectorAll("div.star-rating");

scrollMenuSmooth.forEach(item => {
    item.addEventListener("click", function (e) {
        e.preventDefault();
        const attributeId = item.getAttribute('data-scroll');
        const element = document.getElementById(attributeId);
        let headerHeight = 150;
        if (body.classList.contains('has-top-banner')){
            headerHeight = 200;
        }
        window.scrollTo(0, element.offsetTop - headerHeight);
    })
});


const setMenuScrollFixed = () => {
    if (window.pageYOffset > 700 && window.innerWidth > 992) {
        navbarMenuScroll.classList.add('fixed-menu');
        productNameFixed.classList.add("show");
    } else {
        navbarMenuScroll.classList.remove('fixed-menu');
        productNameFixed.classList.remove("show");
    }
    if (this.scrollY > 120) {
        fixedMenuTopResponsive.classList.add('show-menu');
    } else {
        fixedMenuTopResponsive.classList.remove('show-menu');
    }
    scrollMenuSmooth.forEach(item => {
        if (item.classList.contains('active')) {
            let namePosition = item.querySelector('.text').innerHTML;
            textPagePosition.innerHTML = `${namePosition}`;
        }
    })
}

for (let i = 0; i < starRatingWrapper.length; i++) {
    for (let j = 0; j < starRatingWrapper[i].getAttribute('star-score'); j++) {
        starRatingWrapper[i].children[j].classList.add("fa-star");
    }
}

const handleClickSellersResponsive = () => {
    sellerToggleResponsive.classList.add('show-seller');
    document.body.style.overflowY = "hidden";
}

const handleClickBackSeller = () => {
    sellerToggleResponsive.classList.remove('show-seller');
    document.body.style.overflowY = "auto";
}

const handleClickMoreTextStudyProduct = () => {
    studyProductToggleResponsive.classList.add('show-study-product');
    document.body.style.overflowY = "hidden";
}

const handleClickBackStudyProduct = () => {
    studyProductToggleResponsive.classList.remove('show-study-product');
    document.body.style.overflowY = "auto";
}

const handleClickButtonShare = () => {
    bgModalDesktop.classList.add('show');
    shareWrapperModal.forEach(item => {
        item.classList.add("show-share");
    })
}
const handleClickBgModal = () => {
    bgModalDesktop.classList.remove('show');
    shareWrapperModal.forEach(item => {
        item.classList.remove("show-share");
    })
}

function addFavorite() {
    let productId = this.getAttribute('data-product-id');
    let likeIcon = this.querySelector('.like');
    let dislikeIcon = this.querySelector('.dislike');
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            let response = JSON.parse(this.response);
            if (response.type === 'like') {
                likeIcon.classList.remove('d-none');
                dislikeIcon.classList.add('d-none');
            } else {
                likeIcon.classList.add('d-none');
                dislikeIcon.classList.remove('d-none');
            }
        }
    };
    xhttp.open("POST", '/products/' + productId + '/like');
    xhttp.setRequestHeader("X-CSRF-TOKEN", csrfToken);
    xhttp.setRequestHeader("accept", 'application/json');
    xhttp.send();
}

/*data-bs-spy="scroll" data-bs-target="#nav-bar-single-page" data-bs-offset="500"*/
if (navBarSinglePage) {
    document.body.setAttribute("data-bs-spy", "scroll");
    document.body.setAttribute("data-bs-target", "#nav-bar-single-page");
    document.body.setAttribute("data-bs-offset", "500");
}

window.addEventListener('scroll', setMenuScrollFixed);
if (openSellersResponsive) {
    openSellersResponsive.addEventListener('click', handleClickSellersResponsive);
}
if (backSellerResponsive) {
    backSellerResponsive.addEventListener("click", handleClickBackSeller)
}
if (buttonMoreTextStudyProduct) {
    buttonMoreTextStudyProduct.addEventListener('click', handleClickMoreTextStudyProduct);
}
if (backStudyProductResponsive) {
    backStudyProductResponsive.addEventListener('click', handleClickBackStudyProduct);
}
shareButton.forEach(item => {
    item.addEventListener('click', function () {
        handleClickButtonShare();
    })
});
bgModalDesktop.addEventListener('click', handleClickBgModal);
buttonCloseShareModal.addEventListener('click', handleClickBgModal);

addFavoriteButton.forEach(item => {
    item.addEventListener('click', addFavorite)
});

copyProductUrl.addEventListener('click', function (event) {
    let copyUrl = this.querySelector('input');
    copyUrl.focus();
    copyUrl.select();
    document.execCommand('copy');
    this.querySelector('span').innerHTML = 'کپی شد!';
});

let pageIFrameElements = $(".text-content iframe, .content-study-product-responsive iframe");
function resize_iframes(){
    pageIFrameElements.each(function (i, v) {

        let currentContainer = v.closest('.iframe-container');

        if (currentContainer == null && v.getAttribute("src").indexOf("infogram") == -1) {
            let containerDiv = $('<div class="xpx-md-10 xpx-xl-20 iframe-container"></div>');
            containerDiv.insertAfter(v);
            $(v).appendTo(containerDiv);
            v.style.maxWidth = '100%'
        }

        if(v.getAttribute("src").indexOf("infogram") == -1){
            v.setAttribute('allowfullscreen', 'true');
        }

        w = v.getAttribute('width');
        h = v.getAttribute('height');
        if(!w){
            w = 1280;
            h = 720;
            v.setAttribute('width', w);
            v.setAttribute('height', h);
        }

        pw = $(v).width();
        $(v).css({"height":h/w*pw});
        v.style.height = (h / w * pw).toString() + 'px';
    })
}
resize_iframes();
$(window).resize(function () {
    resize_iframes();
});

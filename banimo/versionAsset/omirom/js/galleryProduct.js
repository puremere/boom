const bgModalGallery = document.querySelector(".bg-gallery-modal");
const thumSwiperResponsive = document.querySelectorAll(".img-box-responsive");
const modalResponsiveGallery = document.querySelector(".modal-gallery-responsive");
const swiperWrapperModalResponsive = document.querySelector(".swiper-modal-responsive-wrapper");
const swiperBigImageModal = document.querySelector(".swiper-big-modal-image");
const toggleResponsiveScrollMenu = document.getElementById("toggle-responsive-scroll-menu");
const scrollMenuBoxResponsive = document.querySelector(".navbar-menu-scroll-responsive");
const listNavbarClick = document.querySelectorAll("#nav-bar-single-page ul li");


let productGallerySwiperResponsive = new Swiper(".gallery-swiper-image-responsive", {
    slidesPerView: 'auto',
    spaceBetween: 8,
    slidesPerGroup: 2,
    navigation: {
        nextEl: ".link-products-gallery-next-responsive",
        prevEl: ".link-products-gallery-prev-responsive",
    },
});


let swiperThumGalleryModal = new Swiper(".thum-gallery-modal", {
    spaceBetween: 10,
    slidesPerView: 'auto',
    slideToClickedSlide: true,
    centeredSlides: true,

});
let productGalleryResponsiveModal = new Swiper(".gallery-responsive-modal-swiper", {
    thumbs: {
        swiper: productGallerySwiperResponsive,
    },
    sslidesPerView: 1,
});


const openGalleryResponsive = () => {
    bgModalGallery.classList.add("show");
    modalResponsiveGallery.classList.add("active-modal");

}
const closeGalleryResponsive = () => {
    bgModalGallery.classList.remove("show");
    modalResponsiveGallery.classList.remove("active-modal")
}
const srcThumResponsive = [];
const altThumResponsive = [];
const slideBoxClass = [];
const dataModalImage = [];
let itemIdElement;
thumSwiperResponsive.forEach(item => {
    const srcItem = item.querySelector('img').getAttribute('src');
    const altItem = item.querySelector('img').getAttribute('alt');
    const itemClassName = item.getAttribute('class');
    const itemDataModal = item.getAttribute('data-modal-image');
    dataModalImage.push(itemDataModal);
    slideBoxClass.push(itemClassName);
    srcThumResponsive.push(srcItem);
    altThumResponsive.push(altItem);
    item.addEventListener("click", function () {
        itemIdElement = item.getAttribute('id');
        openGalleryResponsive();
        if (modalResponsiveGallery.classList.contains("active-modal")) {
            history.pushState(null, document.title, location.href);
            window.addEventListener('popstate', function (event) {
                closeGalleryResponsive();
            });
        }
        setTimeout(function () {
            productGalleryResponsiveModal.controller.control = swiperThumGalleryModal;
            swiperThumGalleryModal.controller.control = productGalleryResponsiveModal;
        }, 500);
    });
    bgModalGallery.addEventListener("click", function () {
        closeGalleryResponsive();
    });
});

for (let i = 0; i < srcThumResponsive.length; i++) {
    const swiperSlide = document.createElement('div');
    swiperSlide.classList.add('swiper-slide');
    const swiperBox = document.createElement('div');

    swiperBox.setAttribute('class', slideBoxClass[i]);

    const imgSwiperThum = document.createElement('img');
    imgSwiperThum.setAttribute('src', srcThumResponsive[i]);
    if (altThumResponsive[i]) {
        imgSwiperThum.setAttribute('alt', altThumResponsive[i]);
    }
    swiperBox.appendChild(imgSwiperThum);
    swiperSlide.appendChild(swiperBox);
    swiperWrapperModalResponsive.appendChild(swiperSlide);
}

for (let i = 0; i < dataModalImage.length; i++) {
    const swiperModalBox = document.createElement('div');
    swiperModalBox.classList.add('swiper-slide');
    const swiperBox = document.createElement('div');
    swiperBox.setAttribute('class', slideBoxClass[i]);
    let imageModalBox;
    if (swiperBox.classList.contains('video-play-responsive')) {
        imageModalBox = document.createElement("iframe");
        imageModalBox.setAttribute('class', 'iframe-video');
    } else {
        imageModalBox = document.createElement("img");
    }
    swiperModalBox.appendChild(swiperBox);
    imageModalBox.setAttribute('src', dataModalImage[i]);
    swiperBox.appendChild(imageModalBox);
    swiperBigImageModal.appendChild(swiperModalBox);
    productGalleryResponsiveModal.on('transitionEnd', function () {
        let videoSrc = imageModalBox.src;
        imageModalBox.src = videoSrc;
    });
    bgModalGallery.addEventListener('click', function () {
        let videoSrc = imageModalBox.src;
        imageModalBox.src = videoSrc;
    });
}

const handleClickToggleScrollMenu = () => {
    if (scrollMenuBoxResponsive.classList.contains('show-responsive-menu')) {
        bgModalGallery.classList.remove("show");
        scrollMenuBoxResponsive.classList.remove('show-responsive-menu');
    } else {
        bgModalGallery.classList.add("show");
        scrollMenuBoxResponsive.classList.add('show-responsive-menu');
    }
    bgModalGallery.addEventListener('click', function () {
        bgModalGallery.classList.remove("show");
        scrollMenuBoxResponsive.classList.remove('show-responsive-menu');
    });
}
listNavbarClick.forEach(item => {
    item.addEventListener('click', function () {
        bgModalGallery.classList.remove("show");
        scrollMenuBoxResponsive.classList.remove('show-responsive-menu');
    });
})

toggleResponsiveScrollMenu.addEventListener("click", handleClickToggleScrollMenu);

let productsListSwiper = new Swiper(".list-products-swiper", {
    slidesPerView: 'auto',
    spaceBetween: 8,
    slidesPerGroup: 5,
    navigation: {
        nextEl: ".link-products-list-next",
        prevEl: ".link-products-list-prev",
    },
    breakpoints: {
        100: {
            slidesPerView: 'auto',
            spaceBetween: 6,
            slidesPerGroup: 1,
        },
        768: {
            slidesPerView: 'auto',
            spaceBetween: 8,
            slidesPerGroup: 5,
        }
    }
});

let studyProductSlide = new Swiper(".img-study-product", {
    slidesPerView: "auto",
    spaceBetween: 16,
    scrollbar: {
        el: ".swiper-scrollbar-study-product",
        hide: false,
    },
});

let productCardsSwiper = new Swiper(".product-card-swiper", {
    slidesPerView: 3.8,
    spaceBetween: 24,
    breakpoints: {
        100: {
            slidesPerView: 1.2,
            spaceBetween: 16,
        },
        500: {
            slidesPerView: 1.8,
            spaceBetween: 16,
        },
        768: {
            slidesPerView: 2.5,
            spaceBetween: 16,
        },
        992: {
            slidesPerView: 3,
            spaceBetween: 24,
        },
        1400: {
            slidesPerView: 4,
            spaceBetween: 24,
        }
    },
    scrollbar: {
        el: ".swiper-scrollbar-card",
        hide: false,
    },

});

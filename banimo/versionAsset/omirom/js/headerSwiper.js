
let headerLinksSwiper = new Swiper(".header-swiper", {
    slidesPerView:10,
    spaceBetween: 16,
    slidesPerGroup: 5,
    navigation: {
        nextEl: ".link-header-button-next",
        prevEl: ".link-header-button-prev",
    },
    breakpoints:{
        992:{
            slidesPerView:8,
            spaceBetween: 16,
        },
        1200:{
            slidesPerView:9,
            spaceBetween: 16,
        },
        1400:{
            slidesPerView:10,
            spaceBetween: 16,
        }
    }
});
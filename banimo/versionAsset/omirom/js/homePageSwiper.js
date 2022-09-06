const topSlideWrapper = document.getElementById("top-slider-wrapper");
const changeBgTopSlide = () => {
    const slideActive = document.querySelector(".top-slider .swiper-slide.swiper-slide-active");
    const attrBg = slideActive.getAttribute("data-color");
    const attrImage = slideActive.getAttribute("data-image");

    topSlideWrapper.style.background = !!attrBg ? attrBg:'#000000';
    if (attrImage){
        topSlideWrapper.style.backgroundImage = 'url('+attrImage+')';
    }
}


let topGalleryThum = new Swiper(".swiper-gallery", {
    spaceBetween: 16,
    slidesPerView: "auto",
    watchSlidesProgress: true,

});
let topGallerySwiper = new Swiper(".mySwiper2", {
    spaceBetween: 10,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    thumbs: {
        swiper: topGalleryThum,
    },
    autoplay: {
        delay: 5000
    }
});
topGallerySwiper.on('transitionEnd', changeBgTopSlide);


let productsSwiper = new Swiper(".product-slider-header-swiper", {
    slidesPerView: 3,
    spaceBetween: 24,
    breakpoints: {
        100: {
            slidesPerView: 1.15,
            spaceBetween: 16,
        },
        992: {
            slidesPerView: 3,
            spaceBetween: 16,
        }
    }
});

let headerResponsiveSwiper = new Swiper(".header-slider-responsive", {
    slidesPerView: 1,
    spaceBetween: 10,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    autoplay: {
        delay: 5000,
    },
});
let linkResponsiveTopSwiper = new Swiper(".link-slider-responsive", {
    slidesPerView: 6.5,
    spaceBetween: 8,
    breakpoints: {
        100:{
            slidesPerView: 3.2,
            spaceBetween: 8,
        },
        320: {
            slidesPerView: 4.5,
            spaceBetween: 8,
        },
        480: {
            slidesPerView: 5.2,
            spaceBetween: 8,
        },
        768: {
            slidesPerView: 6.5,
            spaceBetween: 8,
        },
    }
});
let brandsSwiper = new Swiper(".brads-swiper ", {
    slidesPerView: 8,
    spaceBetween: 24,
    speed: 15000,
    loop: true,
    autoplay: {
        delay: 0,
    },
    breakpoints:{
        100:{
            slidesPerView: 4,
            spaceBetween: 16,
        },
        550:{
            slidesPerView: 8,
            spaceBetween: 16,
        }
    }
});
let newProductsSwiper = new Swiper(".new-products-swiper", {
    slidesPerView: 6.5,
    spaceBetween: 24,
    speed: 15000,
    loop: true,
    autoplay: {
        delay: 0,
    },
    breakpoints: {
        100: {
            slidesPerView: 1.7,
            spaceBetween: 16,
        },
        350: {
            slidesPerView: 2.1,
            spaceBetween: 16,
        },
        460: {
            slidesPerView: 2.7,
            spaceBetween: 16,
        },
        768: {
            slidesPerView: 3.5,
            spaceBetween: 16,
        },
        992: {
            slidesPerView: 4.5,
            spaceBetween: 24,
        },
        1200: {
            slidesPerView: 5.5,
            spaceBetween: 24,
        },
        1400: {
            slidesPerView: 6.5,
            spaceBetween: 24,
        }
    },
});

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

let middleBannerSwiper = new Swiper(".middle-banner", {
    slidesPerView: 1.5,
    spaceBetween: 16,
    breakpoints: {
        100: {
            slidesPerView: 1.05,
            spaceBetween: 8,
        },
        558: {
            slidesPerView: 1.2,
            spaceBetween: 16,
        },
        768: {
            slidesPerView: 1.5,
            spaceBetween: 16,
        },
    }
});

let numberedProductsSwiper = new Swiper(".numbered-products-swiper", {
    slidesPerView: 3.7,
    spaceBetween: 48,
    breakpoints: {
        100:{
            slidesPerView: 1.3,
            spaceBetween: 24,
        },
        330:{
            slidesPerView: 1.55,
            spaceBetween: 24,
        },
        390:{
            slidesPerView: 1.8,
            spaceBetween: 24,
        },
        435:{
            slidesPerView: 2,
            spaceBetween: 24,
        },
        480:{
            slidesPerView: 2.2,
            spaceBetween: 24,
        },
        550: {
            slidesPerView: 2.4,
            spaceBetween: 24,
        },
        768: {
            slidesPerView: 2,
            spaceBetween: 48,
        },
        992: {
            slidesPerView: 2.8,
            spaceBetween: 48,
        },
        1200: {
            slidesPerView: 3.2,
            spaceBetween: 48,
        },
        1400: {
            slidesPerView: 3.7,
            spaceBetween: 48,
        }
    }
});

let numberedProductsVisitedSwiper = new Swiper(".numbered-products-visited-swiper", {
    slidesPerView: 3.7,
    spaceBetween: 48,
    breakpoints: {
        100:{
            slidesPerView: 1.3,
            spaceBetween: 24,
        },
        330:{
            slidesPerView: 1.55,
            spaceBetween: 24,
        },
        390:{
            slidesPerView: 1.8,
            spaceBetween: 24,
        },
        435:{
            slidesPerView: 2,
            spaceBetween: 24,
        },
        480:{
            slidesPerView: 2.2,
            spaceBetween: 24,
        },
        550: {
            slidesPerView: 2.4,
            spaceBetween: 24,
        },
        768: {
            slidesPerView: 2,
            spaceBetween: 48,
        },
        992: {
            slidesPerView: 2.8,
            spaceBetween: 48,
        },
        1200: {
            slidesPerView: 3.2,
            spaceBetween: 48,
        },
        1400: {
            slidesPerView: 3.7,
            spaceBetween: 48,
        }
    }
});


let specialPackagesSwiper = new Swiper(".special-packages-swiper ", {
    slidesPerView: 'auto',
    spaceBetween: 24,
    speed: 500,
    scrollbar: {
        el: ".swiper-scrollbar-packages",
        hide: false,
    },
    breakpoints: {
        420: {
            slidesPerView: 'auto',
            spaceBetween: 16,
        },
        768: {
            slidesPerView: 'auto',
            spaceBetween: 24,
        }
    }
});

let latestNewsSwiper = new Swiper(".latest-news-swiper  ", {
    slidesPerView: "auto",
    spaceBetween: 16,
    scrollbar: {
        el: ".swiper-scrollbar-news",
        hide: false,
    },
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
    }
});

window.addEventListener('load',changeBgTopSlide);


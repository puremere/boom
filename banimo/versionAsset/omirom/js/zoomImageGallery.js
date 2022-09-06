const imgZoom = document.querySelectorAll(".img-zoom-event--g");
const elementShowImage = document.getElementById("overlay");
const videoPlays = document.querySelectorAll(".iframe-video");

let productGallerySwiper = new Swiper(".gallery-swiper-image", {
    slidesPerView: 'auto',
    spaceBetween: 14,
    navigation: {
        nextEl: ".link-products-gallery-next",
        prevEl: ".link-products-gallery-prev",
    },
});
let imgZoomSwiperDesktop = new Swiper(".swiper-image-zoom-desktop", {
    slidesPerView: 1,
    spaceBetween: 14,
    thumbs: {
        swiper: productGallerySwiper
    }
});
const setImageZoomBackground = () => {
    imgZoom.forEach(item => {
        const srcImage = item.parentElement.parentElement;
        if (srcImage.classList.contains('swiper-slide-active')) {
            const srcImage = item.getAttribute('src');
            elementShowImage.style.backgroundImage = 'url(' + srcImage + ')';
        }
    })
}
setImageZoomBackground();
imgZoomSwiperDesktop.on('transitionEnd', function () {
    setImageZoomBackground();
    videoPlays.forEach(video => {
        let iframeSrc = video.src;
        video.src = iframeSrc;
    })
})

function zoomIn(event) {
    elementShowImage.style.display = "inline-block";
    imgZoom.forEach(item => {
        let posX = event.offsetX ? (event.offsetX) : event.pageX - item.offsetLeft;
        let posY = event.offsetY ? (event.offsetY) : event.pageY - item.offsetTop;
        elementShowImage.style.backgroundPosition = (-posX / 5 + 10) + "px " + (-posY / 5 + 10) + "px";
        /*        imgZoomSwiperDesktop.on('transitionEnd', function () {*/
        /*         const srcImage = item.getAttribute('src');
             console.log(srcImage);
                 elementShowImage.setAttribute('src',srcImage);*/
        /*    });*/
    })
}

function zoomOut() {
    elementShowImage.style.display = "none";
}

imgZoom.forEach(item => {
    item.addEventListener("mousemove", function (event) {
        zoomIn(event);
    });
    item.addEventListener("mouseout", function () {
        zoomOut();
    });
});

elementShowImage.addEventListener("mousemove", zoomOut);

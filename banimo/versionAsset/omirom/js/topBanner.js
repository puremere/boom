const topBannerWrapper = document.querySelector(".top-header-banner-wrapper");
const headerWrapper = document.getElementById("header");
if (topBannerWrapper) {
    const heightBanner = topBannerWrapper.offsetHeight;

    const handleHeaderPosition = () => {
        if (window.scrollY > heightBanner) {
            headerWrapper.classList.add("fixed-menu")
        } else {
            headerWrapper.classList.remove("fixed-menu")
        }
    }

    window.addEventListener("scroll", handleHeaderPosition);

}

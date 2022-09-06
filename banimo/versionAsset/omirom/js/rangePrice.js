const minPrice = document.querySelectorAll(".min-price-slider");
const maxPrice = document.querySelectorAll(".max-price-slider");


function setMinPrice(price) {
    minPrice.forEach(minP => {
        minP.innerHTML = price.toLocaleString('fa-IR').toPersianDigits() + " " + "ت";
    })
}

function setMaxPrice(price) {
    maxPrice.forEach(minP => {
        minP.innerHTML = price.toLocaleString('fa-IR').toPersianDigits() + " " + "ت";
    })
}


function initPriceRanges() {

    document.querySelectorAll('.price-slider input[type=range]').forEach(function (el) {
        el.oninput = function () {

            let parent = this.closest('.price-slider');
            let rangeS = parent.querySelectorAll("input[type=range]");


            var slide1 = parseFloat(rangeS[0].value),
                slide2 = parseFloat(rangeS[1].value);

            if (slide1 > slide2) {
                [slide1, slide2] = [slide2, slide1];
            }

            setMinPrice(slide1);
            setMaxPrice(slide2);

        }
    });

}
initPriceRanges();
document.querySelector('.price-slider input[type=range]').dispatchEvent(new Event('input') );
const priceProduct = document.querySelectorAll(".total-price");
const discount = document.querySelectorAll(".discount");
const phoneNumberProfile = document.getElementById("phone-number-profile");
const pageLink = document.querySelector('.pagination');
const totalScore = document.querySelectorAll(".total-score");
const infoProducts = document.querySelectorAll(".info-product");
const showMinPriceSlider = document.querySelector(".show-min-price");
const showMaxPriceSlider = document.querySelector(".show-max-price");
const scoreSeller = document.querySelectorAll('.score-seller,.score-sellers');

persian = {0: '۰', 1: '۱', 2: '۲', 3: '۳', 4: '۴', 5: '۵', 6: '۶', 7: '۷', 8: '۸', 9: '۹'};

function traverse(el) {
    if (el.nodeType == 3) {
        let list = el.data.match(/[0-9]/g);
        if (list != null && list.length != 0) {
            for (let i = 0; i < list.length; i++)
                el.data = el.data.replace(list[i], persian[list[i]]);
        }
    }
    for (let i = 0; i < el.childNodes.length; i++) {
        traverse(el.childNodes[i]);
    }
}

const loopNumber = (name) => {
    for (let i = 0; i < name.length; i++) {
        traverse(name[i]);
    }
}

if (pageLink) {
    traverse(pageLink);
}
if (priceProduct || discount) {
    loopNumber(discount);
    loopNumber(priceProduct);
}

if (totalScore) {
    loopNumber(totalScore);
}

if (infoProducts) {
    loopNumber(infoProducts);
}

if (phoneNumberProfile) {
    traverse(phoneNumberProfile);
}
if (showMinPriceSlider) {
    traverse(showMinPriceSlider);
}
if (showMaxPriceSlider) {
    traverse(showMaxPriceSlider);
}
if (scoreSeller) {
    loopNumber(scoreSeller);
}


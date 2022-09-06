const closeBuyPageButton = document.querySelector(".title-buy-page-header");
const buttonBuyProduct = document.getElementById("button-buy-product");
const buyPageWrapper = document.querySelector(".buy-page-wrapper");
const singlePageWrapper = document.querySelector(".single-page");
const selectionColorBuy = document.querySelectorAll(".selection-color");
const selectionWarrantyBuy = document.querySelectorAll(".selection-warranty");
const selectionWarrantyBuyResponsive = document.querySelectorAll(".responsive-selection-warranty");
const selectionColorBuyResponsive = document.querySelectorAll(".responsive-selection-color");
const accessoriesColorBox = document.querySelectorAll(".accessories-color-box");
const nextStepButton = document.getElementById("next-step");
const nextStepButtonResponsive = document.querySelectorAll(".next-page-buy");
const buttonChangeBuyPage = document.getElementById("button-change-buy-page");
const upScrollPage = document.getElementById("up-scroll-page");
const warrantySelectionBox = document.getElementById("warranty-scroll-buy-page");
const accessoriesSelectionBox = document.querySelector("#accessories-scroll-buy-page");
const prevStepButton = document.getElementById("previous-step");
const prevStepButtonResponsive = document.querySelectorAll(".previous-step-responsive");
const addToCartButton = document.querySelectorAll(".add-to-cart");
const productColors = document.querySelectorAll(".range-box-color");
const sellers = document.querySelectorAll(".seller-item");

let productGallerySwiperBuyPage = new Swiper(".gallery-swiper-buy-page", {
    slidesPerView: 'auto',
    spaceBetween: 14,
    navigation: {
        nextEl: ".link-products-gallery-next-buy-page",
        prevEl: ".link-products-gallery-prev-buy-page",
    },
});
let imgZoomSwiperDesktopBuyPage = new Swiper(".swiper-image-buy-page", {
    slidesPerView: 1,
    spaceBetween: 14,
    thumbs: {
        swiper: productGallerySwiperBuyPage
    }
});
imgZoomSwiperDesktopBuyPage.on('transitionEnd', function () {
    setImageZoomBackground();
    videoPlays.forEach(video => {
        let iframeSrc = video.src;
        video.src = iframeSrc;
    })
});

if (upScrollPage) {
    upScrollPage.addEventListener("click", function () {
        buyPageWrapper.scrollTo(0, 0);
        document.querySelectorAll('.buy-step-element').forEach(item => {
            item.classList.remove('active');
        });
        let firstStep = document.querySelector('.buy-step-element');
        firstStep.classList.add('active');
        let nextStep = firstStep.nextElementSibling;
        prevStepButton.setAttribute('href', '#');
        nextStepButton.setAttribute('href', '#' + nextStep.getAttribute('id'));

        document.getElementById('step-progress-buy-page').querySelectorAll('.active').forEach((item, key) => {
            if (key === 0) {
                return false;
            }
            item.classList.remove('active');
        });
    });
}
function formatPrice(price){
    return new Intl.NumberFormat('fa-IR').format(price);
}
const handleChangeSellersBox = (color) => {
    if (sellers.length === 0) {
        return false;
    }
    let currentData;
    currentData = null;
    sellers.forEach(item => {
        item.classList.add('d-none');
        if (item.getAttribute('data-color') == color) {
            item.classList.remove('d-none');
            let newData;
            newData = {
                finalPrice: item.getAttribute('data-price'),
                regularPrice: item.getAttribute('data-regular-price'),
                seller: item.getAttribute('data-seller'),
                score: item.getAttribute('data-score'),
                delay: item.getAttribute('data-delay'),
                warranty: item.getAttribute('data-warranty')
            };
            if(currentData == null || parseInt(newData.finalPrice) < parseInt(currentData.finalPrice)){
                currentData = {...newData};
            }
        }
    });

    $('.seller-information .seller-name').text(currentData.seller);
    $('.seller-information .score').text('عملکرد' + ' ' + currentData.score + ' از ' + '۵');
    $('.seller-information .time-send-product').text('ارسال از' + ' ' + currentData.delay + ' ' + 'روز کاری آینده');
    $('.seller-information .warranty-text').text( currentData.warranty);
    $('#button-buy-product .text-buy').text('خرید' + ' ' + formatPrice(currentData.finalPrice) + ' ت');

    $('.seller-information-responsive .seller-name-responsive').text(currentData.seller);
    $('.seller-information-responsive .score').text('عملکرد' + ' ' + currentData.score + ' از ' + '۵');
    $('.seller-information-responsive .time-send-product').text('ارسال از' + ' ' + currentData.delay + ' ' + 'روز کاری آینده');
    $('.seller-information-responsive .warranty-text').text( currentData.warranty);

    if(parseInt(currentData.finalPrice) < parseInt(currentData.regularPrice)){
        $('.text-price-responsive del').text(formatPrice(currentData.regularPrice));
        $('.text-price-responsive span').text(formatPrice(currentData.finalPrice));
    }else{
        $('.text-price-responsive del').text('');
        $('.text-price-responsive span').text(formatPrice(currentData.finalPrice));
    }


    document.querySelectorAll('.selection-color').forEach((item) => {
        item.classList.remove('active');
        if (item.getAttribute('data-color') == color) {
            item.classList.add('active');
            handleChangeSelectedVariation(item);
        }
    });

    document.querySelectorAll('.responsive-selection-color').forEach((item) => {
        item.classList.remove('active');
        if (item.getAttribute('data-color') == color) {
            item.classList.add('active');
            handleChangeSelectedVariation(item);
        }
    });

};

productColors.forEach(item => {
    let activeColor;
    item.addEventListener("click", function () {
        item.parentElement.querySelector('.active').classList.remove('active');
        item.classList.add("active");
        activeColor = item.getAttribute("data-product-color");
        handleChangeSellersBox(activeColor);
        handleChangeSelectedVariation(item);
    });
});

function handleChangeSelectedVariation(item) {
    document.querySelectorAll('.selected-price').forEach(element => {
        element.innerHTML = item.querySelector('.variation-price').innerHTML;
    });
    document.querySelector('#add-to-cart-form input[name="variation"]').value = item.getAttribute('data-variation');
}

function handleChangeWarrantiesList(item) {
    let activeColor = item.getAttribute('data-color');
    document.querySelectorAll('.selection-warranty').forEach(element => {
        element.classList.remove('active');
        if (element.getAttribute('data-color') !== activeColor) {
            element.classList.add('d-none');
        }
    });
    document.querySelector('.selection-warranty:not(.d-none)').classList.add('active');
}

function resetWarrantiesList() {
    document.querySelectorAll('.selection-warranty').forEach(element => {
        element.classList.remove('d-none');
    });
}

function handleChangeWarrantiesListResponsive(item) {
    let activeColor = item.getAttribute('data-color');
    document.querySelectorAll('.responsive-selection-warranty').forEach(element => {
        element.classList.remove('active');
        if (element.getAttribute('data-color') !== activeColor) {
            element.classList.add('d-none');
        }
    });
    document.querySelector('.responsive-selection-warranty:not(.d-none)').classList.add('active');
}

function resetWarrantiesListResponsive() {
    document.querySelectorAll('.responsive-selection-warranty').forEach(element => {
        element.classList.remove('d-none');
    });
}

const handleClickButtonBuyProduct = () => {
    let currentStep = document.querySelector('.buy-step-element.active');
    if (!currentStep) {
        document.getElementById('add-to-cart-form').submit();
        return;
    }
    let nextStep = currentStep.nextElementSibling;
    if (!nextStep) {
        document.getElementById('add-to-cart-form').submit();
        return;
    }
    document.body.style.overflowY = "hidden";
    buyPageWrapper.classList.add("show-buy");
    singlePageWrapper.classList.add("buy-page");

}

const handleCloseBuyPage = () => {
    document.body.style.overflowY = "auto";
    buyPageWrapper.classList.remove("show-buy");
    singlePageWrapper.classList.remove("buy-page");
}

closeBuyPageButton.addEventListener('click', handleCloseBuyPage);

selectionColorBuy.forEach(item => {
    item.addEventListener("click", function () {
        item.parentElement.querySelector('.active').classList.remove('active');
        item.classList.add("active");
        handleChangeSelectedVariation(item);
        handleClickNextStep();
        buyPageWrapper.scrollTo(0, warrantySelectionBox.offsetTop);
    })
});

selectionWarrantyBuy.forEach(item => {
    item.addEventListener("click", function () {
        handleChangeSelectedVariation(item);
        handleClickNextStep();
        item.parentElement.querySelector('.active').classList.remove('active');
        item.classList.add("active");
        if (accessoriesSelectionBox instanceof HTMLElement) {
            buyPageWrapper.scrollTo(0, accessoriesSelectionBox.offsetTop);
        }
    })
});


selectionColorBuyResponsive.forEach(item => {
    item.addEventListener("click", function () {
        item.parentElement.querySelector('.active').classList.remove('active');
        item.classList.add("active");
        handleChangeSelectedVariation(item);
        resetWarrantiesListResponsive();
        handleChangeWarrantiesListResponsive(item);
        buyPageWrapper.scrollTo(0, warrantySelectionBox.offsetTop);
    })
});

selectionWarrantyBuyResponsive.forEach(item => {
    item.addEventListener("click", function () {
        handleChangeSelectedVariation(item);
        item.parentElement.querySelector('.active').classList.remove('active');
        item.classList.add("active");
        buyPageWrapper.scrollTo(0, accessoriesSelectionBox.offsetTop);
    })
});


function handleClickNextStep() {
    let currentStep = document.querySelector('.buy-step-element.active');
    currentStep.classList.remove('active');
    resetWarrantiesList();
    handleChangeWarrantiesList(document.querySelector('.selection-color.active'));
    let nextStep = currentStep.nextElementSibling;
    if (nextStep instanceof HTMLElement) {
        nextStepButton.setAttribute('href', '#' + nextStep.getAttribute('id'));
        nextStep.classList.add('active');
        prevStepButton.setAttribute('href', '#' + currentStep.getAttribute('id'));
    } else {
        document.getElementById('add-to-cart-form').submit();
        return;
    }
    document.getElementById('step-progress-buy-page').querySelector('.item:not(.active)').classList.add('active');
}

function handleClickPreviousStep() {
    let currentStep = document.querySelector('.buy-step-element.active');
    let previousStep = currentStep.previousElementSibling;
    if (!previousStep) {
        handleCloseBuyPage();
        return false;
    }
    currentStep.classList.remove('active');
    previousStep.classList.add('active');
    prevStepButton.setAttribute('href', '#' + previousStep.getAttribute('id'));
    nextStepButton.setAttribute('href', '#' + currentStep.getAttribute('id'));

    let activeItems = document.getElementById('step-progress-buy-page').querySelectorAll('.active');
    activeItems[activeItems.length - 1].classList.remove('active');

    if (!previousStep.previousElementSibling) {
        setTimeout(function () {
            buyPageWrapper.scrollTo(0, 0);
        }, 300);
    }
}

nextStepButtonResponsive.forEach(item => {
    item.addEventListener('click', () => {
        let nextStep = item.closest(".buy-step-responsive").nextElementSibling;
        if (nextStep instanceof HTMLElement) {
            nextStep.classList.add('show');
        } else {
            document.getElementById('add-to-cart-form').submit();
        }
    });
});

prevStepButtonResponsive.forEach(item => {
    item.addEventListener('click', () => {
        item.parentElement.classList.remove('show');
        let activeStep = document.querySelector('.buy-step-responsive.show')
        if(activeStep == null){
            document.body.style.overflowY = "auto";
            buyPageWrapper.classList.remove("show-buy");
            singlePageWrapper.classList.remove("buy-page");
        }
    });
});

document.body.onload = function () {
    let currentStep = document.querySelector('.buy-step-element.active');
    if (currentStep) {
        let nextStep = currentStep.nextElementSibling;
        if (nextStep) {
            nextStepButton.setAttribute('href', '#' + nextStep.getAttribute('id'));
        }
    }

    if (buttonBuyProduct) {
        buttonBuyProduct.addEventListener("click", handleClickButtonBuyProduct);
    }
    if (productColors.length > 0) {
        let defaultColor = document.getElementById('default-variation-color').value;
        handleChangeSellersBox(defaultColor);
    }
};

nextStepButton.addEventListener('click', handleClickNextStep);
prevStepButton.addEventListener('click', handleClickPreviousStep);

if (buttonChangeBuyPage) {
    buttonChangeBuyPage.addEventListener('click', () => {

        let currentStep = document.querySelector('.buy-step-element.active');
        if (!currentStep) {
            document.getElementById('add-to-cart-form').submit();
            return;
        }
        document.body.style.overflowY = "hidden";
        buyPageWrapper.classList.add("show-buy");
        singlePageWrapper.classList.add("buy-page");
        let nextStep = currentStep.nextElementSibling;
        if (!nextStep) {
            document.getElementById('add-to-cart-form').submit();
            return;
        }
        document.querySelector('.buy-step-responsive').classList.add('show');
        resetWarrantiesListResponsive();
        handleChangeWarrantiesListResponsive(document.querySelector('.responsive-selection-color.active'));
    });
}


// begin add accessories to cart
function setSelectedVariation(color) {
    const addToCart = color.parentElement.parentElement.parentElement.querySelector('.add-to-cart');
    addToCart.setAttribute('data-variation', color.getAttribute('data-variation'));
}

accessoriesColorBox.forEach(item => {
    item.addEventListener('click', function () {
        item.parentElement.querySelectorAll('.accessories-color-box').forEach(child => {
            child.classList.remove('active');
        });
        item.classList.add('active');
        setSelectedVariation(item);
        item.parentElement.parentElement.parentElement.querySelector('.added-to-cart').classList.add('d-none');
        item.parentElement.parentElement.parentElement.querySelector('.add-cart').classList.remove('d-none');
    });
});

addToCartButton.forEach(item => {
    item.addEventListener('click', async () => {
        const request = await fetch('/cart/add',
            {
                method: 'post',
                body: JSON.stringify({
                    variation: item.getAttribute('data-variation')
                }),
                headers: jsonFormHeaders,
            });
        const result = await request.json();
        if (request.status === 200) {
            item.parentElement.classList.add('d-none');
            item.parentElement.nextElementSibling.classList.remove('d-none');
        } else if (result.code === 422) {
            for (const error of result.errors_list) {
                alert(error);
            }
        }
    });
});

// end add accessories to cart

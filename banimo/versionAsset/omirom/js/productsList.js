const buttonProductList = document.getElementById("card-list-products");
let typeCardProducts = document.getElementById("type-card-products");
let typeCardProductsList = document.getElementById("type-card-products-list");
const starRatingWrapper = document.querySelectorAll("div.star-rating");
const categoryProductFilter = document.getElementById("category-product-filter");
const filterToggleButton = document.getElementById("filter-toggle-button");
const filterToggleOptions = document.getElementById("filter-options");
const iconMoreFilter = document.getElementById("icon-more-filter");
const ButtonToggleFilterResponsive = document.getElementById("button-toggle-filter-responsive");
const wrapperToggleFilterResponsive = document.getElementById("filter-wrapper-responsive");
const bgModalWrapperProduct = document.querySelector(".bg-modal-wrapper-menu-products");
const toggleListFilters = document.querySelectorAll("#filter-wrapper-responsive ul div.toggle-list");
// const loginButtonResponsiveHeader = document.querySelector(".login-button-responsive-header");
// const searchButtonHeader = document.querySelector(".search-btn-header");
const selectButtonFilterResponsive = document.getElementById("select-btn-filter-product-responsive");
const toggleSelectFilterResponsive = document.getElementById("toggle-select-filter-responsive");
const selectFilterProduct = document.querySelectorAll(".change-select");
// const clickDeleteButtonValueSelect = document.querySelectorAll(".delete-button-value-select");
// const changeNumberFilter = document.querySelector(".change-number-filter");
const menuFilterContent = document.querySelector(".top-fixed-menu-wrapper");
const sortSelect = document.getElementById('sort-select');

$('#toggle-select-filter-responsive li').on('click', function(){
    $(this).closest('ul').children('li').removeClass('active');
    $(this).addClass('active');
    $(sortSelect).val($(this).attr('data-value'));
    selectButtonFilterResponsive.querySelector('span').innerHTML = $(this).text();
    handleClickSelectResponsive();
    handleFiltersChanged();
});
selectButtonFilterResponsive.querySelector('span').innerHTML = $('#toggle-select-filter-responsive li.active').text();

$(sortSelect).on('change', function(){
    let liResponsive = document.querySelector('#toggle-select-filter-responsive li[data-value=' + $(this).val() + ']');
    if(liResponsive){
        $(liResponsive).closest('ul').children('li').removeClass('active');
        $(liResponsive).addClass('active');
        selectButtonFilterResponsive.querySelector('span').innerHTML = $(liResponsive).text();
        handleFiltersChanged();
    }
})


$('.filter-brand').select2({
    placeholder: 'برندهای مورد نظر را انتخاب کنید',
    language: "fa"
});

$('.filter-color').select2({
    placeholder: 'رنگ‌های مورد نظر را انتخاب کنید',
    language: "fa"
});

$('.select2-container').addClass('theme-sd');

/*change value select filter*/
let numberFilter = 0;
const deleteValueSelect = (item) => {
    const deleteButton = item.parentElement.querySelector('.delete');
    if (item.value !== "default" || item.value !== "") {
        deleteButton.classList.add('show-delete');
    } else {
        deleteButton.classList.remove('show-delete');
        item.classList.remove("changed");
    }
}

selectFilterProduct.forEach(item => {
    const deleteButton = item.parentElement.querySelector('.delete');
    $(item).on('change', function () {

        item.classList.add("changed");
        if (deleteButton) {
            deleteValueSelect(item);
        }

        if(item.classList.contains('filter-brand')){
            $('.filter-brand').not(item).val($(item).val()).trigger('change.select2');
        }
        if(item.classList.contains('filter-color')){
            $('.filter-color').not(item).val($(item).val()).trigger('change.select2');
        }

        if(item.classList.contains('price-range-min')){
            $('.price-range-min').not(item).val($(item).val());
        }
        if(item.classList.contains('price-range-max')){
            $('.price-range-max').not(item).val($(item).val());
        }

        if(item.classList.contains('filter-in_stock')){
            $('.filter-in_stock').not(item).prop('checked', $(item).prop('checked'));
        }

        if(item.classList.contains('filter-has_discount')){
            $('.filter-has_discount').not(item).prop('checked', $(item).prop('checked'));
        }

        if(item.closest('#filter-wrapper-responsive') == null){
            handleFiltersChanged();
        }

    })
});

// const handleClickDeleteSelectValue = (item) => {
//     const selectInput = item.parentElement.parentElement.querySelector(".change-select");
//     selectInput.value = "";
//     selectInput.classList.remove("changed");
// }
// clickDeleteButtonValueSelect.forEach(item => {
//     item.addEventListener('click', function () {
//         handleClickDeleteSelectValue(item);
//     })
// })


/*change type card show*/
const handleClickCardList = () => {
    typeCardProducts.classList.toggle("d-none");
    typeCardProductsList.classList.toggle("d-none");
    buttonProductList.classList.toggle("change-list");
};

/*handle score star product*/
for (let i = 0; i < starRatingWrapper.length; i++) {
    for (let j = 0; j < starRatingWrapper[i].getAttribute('star-score'); j++) {
        starRatingWrapper[i].children[j].classList.add("fa-star");
    }
}


const handleScrollFilterFix = () => {
    let add = 0;
    if(document.querySelector('body').classList.contains('has-top-banner')){
        add = 48;
    }
    // add += document.querySelector('.bg-products-page').clientHeight;

    if (this.scrollY > add ) {
        categoryProductFilter.classList.add("fixed-top");
    } else {
        categoryProductFilter.classList.remove("fixed-top");
    }
}

/*toggle filter wrapper*/
const handleClickFilterToggleButton = () => {
    filterToggleOptions.classList.toggle("show");
    iconMoreFilter.classList.toggle("down-icon");
    filterToggleButton.classList.toggle("active-filter");
    menuFilterContent.classList.toggle("fixed-content");
}

/*open bgModal */
const openBgModalWrapperProducts = () => {
    bgModalWrapperProduct.classList.add("animate__fadeIn");
    bgModalWrapperProduct.classList.remove("animate__fadeOut");
    bgModalWrapperProduct.style.display = "block";
}
/*close bgModal*/
const closeBgModalWrapperProducts = (timeOut) => {
    bgModalWrapper.classList.remove("animate__fadeIn");
    bgModalWrapper.classList.add("animate__fadeOut");
    setTimeout(function () {
        bgModalWrapper.style.display = "none";
    }, timeOut);
}

/*toggle modal filter responsive*/
const handleToggleFilterResponsive = () => {
    wrapperToggleFilterResponsive.classList.toggle("show");
    toggleSelectFilterResponsive.classList.remove("show");
    ButtonToggleFilterResponsive.classList.toggle("active-filter");
    if (wrapperToggleFilterResponsive.classList.contains("show")) {
        openBgModalWrapperProducts();
        categoryProductFilter.style.zIndex = 1001;
    } else {
        closeBgModalWrapperProducts(600);
        setTimeout(function () {
            categoryProductFilter.style.zIndex = 5;
        }, 600);
    }
}

if (window.innerWidth < 768) {
    categoryProductFilter.classList.remove("container");
} else {
    categoryProductFilter.classList.add("container");
}
const handleResizeWindow = () => {
    if (window.innerWidth < 768) {
        categoryProductFilter.classList.remove("container");
    } else {
        categoryProductFilter.classList.add("container")
    }
};

/*handle open child toggle filter responsive*/
toggleListFilters.forEach(item => {
    item.addEventListener("click", function () {
        let ul = item.parentElement.querySelector("ul");
        if(ul){
            ul.classList.toggle("show-list");
            item.querySelector("i.add-icon").classList.toggle("down-icon");
        }

    });
});

/*close toggle filter products*/
const closeToggleFilterProducts = () => {
    closeBgModalWrapperProducts(600);
    wrapperToggleFilterResponsive.classList.remove("show");
    toggleSelectFilterResponsive.classList.remove("show");
    ButtonToggleFilterResponsive.classList.remove("active-filter");
    setTimeout(function () {
        categoryProductFilter.style.zIndex = 5;
    }, 600);
};
const handleCloseFilterMenu = () => {
    closeToggleFilterProducts();
};

const handleClickLoginButtonHeader = () => {
    wrapperToggleFilterResponsive.classList.remove("show");
    toggleSelectFilterResponsive.classList.remove("show");
    ButtonToggleFilterResponsive.classList.remove("active-filter");
    setTimeout(function () {
        categoryProductFilter.style.zIndex = 5;
    }, 600);
};
const handleClickSearchButtonHeader = () => {
    handleClickLoginButtonHeader();
};

/*click for filter select responsive*/
const handleClickSelectResponsive = () => {
    toggleSelectFilterResponsive.classList.toggle("show");
    wrapperToggleFilterResponsive.classList.remove("show");
    ButtonToggleFilterResponsive.classList.remove("active-filter");
    if (toggleSelectFilterResponsive.classList.contains("show")) {
        openBgModalWrapperProducts();
        categoryProductFilter.style.zIndex = 1001;
    } else {
        closeBgModalWrapperProducts(600);
        setTimeout(function () {
            categoryProductFilter.style.zIndex = 5;
        }, 600);
    }
};


const serialize = function(obj, prefix) {
    var str = [], p;
    for (p in obj) {
        if (obj.hasOwnProperty(p)) {
            var k = prefix ? prefix + "[" + p + "]" : p,
                v = obj[p];
            str.push((v !== null && typeof v === "object") ?
                serialize(v, k) :
                encodeURIComponent(k) + "=" + encodeURIComponent(v));
        }
    }
    return str.join("&");
}

const loadProductList  = (filters, sortBy = 'newest', url = null) => {
    let productsListDivision = document.querySelector('.products-card');
    let listContainer = productsListDivision.parentElement;

    productsListDivision.classList.add('loading');

    let q = $('input[name=current-search]').first().val();
    // Not a click. we generate params from inputs
    if(url == null){
        url = productListRoute;
        let paramData = {};
        if(Object.keys(filters).length > 0){
            paramData.filters = filters;
        }
        if(!!sortBy){
            paramData.sortby = sortBy;
        }
        if(!!q){
            paramData.q = q;
        }
        if(Object.keys(paramData).length > 0){
            url = url + '?' + serialize(paramData);
        }
    }

    $.ajax({
        url: url,
        data: {
            filters: filters,
            q: q,
            partial: 'true',
        },
        success: (response) => {
            productsListDivision.remove();
            listContainer.innerHTML = response;
            typeCardProducts = document.getElementById("type-card-products");
            typeCardProductsList = document.getElementById("type-card-products-list");
            window.history.pushState({"html":response.html, "pageTitle": "some title"},"", url);
        },
        complete: () => {
            productsListDivision.classList.remove('loading');
            window.scrollTo(0, 0);
        }
    });

}

const handleFiltersChanged = () => {

    let filterCount = 0;

    let sortBy = $(sortSelect).val();

    let brand = $(".filter-brand").first().val().filter((i) => {return !!i});
    let color = $(".filter-color").first().val().filter((i) => {return !!i});

    let hasDiscount = $(".filter-has_discount").first().prop('checked');
    let inStock = $(".filter-in_stock").first().prop('checked');

    let filters = {};

    if(hasDiscount){
        filters.discounted = true;
        filterCount++;
    }
    if(inStock){
        filters.in_stock = true;
        filterCount++;
    }

    let range1 = document.querySelector('.price-range-min');
    let range2 = document.querySelector('.price-range-max');

    let selectedRange1 = parseFloat(range1.value);
    let selectedRange2 = parseFloat(range2.value);

    let rangeMin = Math.min(parseFloat(range1.min), parseFloat(range2.min));
    let rangeMax = Math.max(parseFloat(range1.max), parseFloat(range2.max));
    let minPrice, maxPrice;

    [minPrice, maxPrice] = selectedRange1 < selectedRange2 ? [selectedRange1, selectedRange2]:[selectedRange2, selectedRange1];


    if(minPrice > rangeMin){
        filters.min_price = minPrice;
        filterCount++;
    }
    if(maxPrice < rangeMax){
        filters.max_price = maxPrice;
        filterCount++;
    }

    if(brand.length > 0 ){
        console.log('we have brand')
        console.log(brand);
        filters.brand = brand;
        filterCount++;
    }
    if(color.length > 0){
        filters.color = color;
        filterCount++;
    }

    document.querySelectorAll('.number-filter-icon').forEach((element) => {element.innerHTML = filterCount.toLocaleString('fa-IR')});

    loadProductList(filters, sortBy, null)
}

$('.products-list').on('click', '.paginationn a', function(e){
    e.preventDefault();
    loadProductList({}, null, this.getAttribute('href'));
})
const handleClickApplyButton =  () => {
    handleFiltersChanged();
    handleToggleFilterResponsive();
}

const handleClickResetButton =  () => {
    $(wrapperToggleFilterResponsive).find('select').each(function (i, v){
        $(v).val(null).trigger('change');
    })
    // $(wrapperToggleFilterResponsive).find('input[type=checkbox]').prop('checked', false).trigger('change');
    //
    $(wrapperToggleFilterResponsive).find('.price-range-min').val('0').trigger('change');
    $(wrapperToggleFilterResponsive).find('.price-range-max').val($(wrapperToggleFilterResponsive).find('.price-range-max').attr('max')).trigger('change');

    setMinPrice(0);
    setMaxPrice($(wrapperToggleFilterResponsive).find('.price-range-max').attr('max'));
    // $(wrapperToggleFilterResponsive).find('input[type=checkbox]')
    handleFiltersChanged();
    handleToggleFilterResponsive();
}

buttonProductList.addEventListener("click", handleClickCardList);
window.addEventListener("scroll", handleScrollFilterFix);
filterToggleButton.addEventListener("click", handleClickFilterToggleButton);
ButtonToggleFilterResponsive.addEventListener("click", handleToggleFilterResponsive);
window.addEventListener("resize", handleResizeWindow);
bgModalWrapperProduct.addEventListener("click", handleCloseFilterMenu);
// loginButtonResponsiveHeader.addEventListener("click", handleClickLoginButtonHeader);
// searchButtonHeader.addEventListener("click", handleClickSearchButtonHeader);
selectButtonFilterResponsive.addEventListener("click", handleClickSelectResponsive);
document.querySelector('.apply-filters').addEventListener('click', handleClickApplyButton);
document.querySelector('.reset-filters').addEventListener('click', handleClickResetButton);


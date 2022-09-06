const searchBoxInput = document.querySelector("#search-box-input");
const ulProductsSearch = document.getElementById("product-ul-search");
const ulBlogsSearch = document.getElementById("blog-ul-search");
const iconSearchFocus = document.querySelector(".search-icon-header");
const searchResultWrapper = document.querySelector(".result-search-wrapper-header");


let productSearchAbortController = new AbortController();
let blogSearchAbortController = new AbortController();
let abortInitialized = false;

const SSM = {
    currentText: '',
    currentTimeout: 0,

    init: function () {
        searchBoxInput.oninput = function (event) {
            let text = searchBoxInput.value.trim();
            if (text != SSM.currentText) {
                clearTimeout(SSM.currentTimeout);
                // console.log(productSearchAbortController.signal)
                SSM.currentTimeout = setTimeout(function () {
                    SSM.search(text);
                }, 500);
                SSM.currentText = text;
            }
        }
    },
    search: async function (text) {
        if (searchBoxInput.value.length>0) {
            searchResultWrapper.classList.add("active");
        } else {
            searchResultWrapper.classList.remove("active");
        }

        if(abortInitialized){
            productSearchAbortController.abort();
            blogSearchAbortController.abort();
        }
        abortInitialized = true;


        productSearchAbortController = new AbortController();
        blogSearchAbortController = new AbortController();

        fetch('/search/?count=4&q=' + encodeURIComponent(text), {
            signal: productSearchAbortController.signal,
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(response => response.json()).then(data => {
                let list = ulProductsSearch;
                list.innerHtml = '';
                list.textContent = '';
                data.products.forEach(function (product) {
                    let list_item = document.createElement("li");
                    let list_link = document.createElement("a");
                    list_link.setAttribute('href', product.url);
                    ulProductsSearch.appendChild(list_item);
                    list_item.appendChild(list_link);
                    let imgSearch = document.createElement("img");
                    imgSearch.classList.add("products");
                    imgSearch.setAttribute('src', product.main_image.thumbnail)
                    let contentName = document.createElement("span");
                    contentName.innerHTML = product.name;
                    list_link.appendChild(imgSearch);
                    list_link.appendChild(contentName);
                });
                document.getElementById('search-all-products').setAttribute('href', "/search" + '?q=' + encodeURI(text));

            });

        fetch("https://saymandigital.com/?post_type=post&s=" + encodeURIComponent(text) + "&is_ajax=1", {
            method: 'get',
            signal: blogSearchAbortController.signal,
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                let list = ulBlogsSearch;
                list.innerHtml = '';
                list.textContent = '';
                data.posts.slice(0, 4).forEach(function (post) {
                    let list_item = document.createElement("li");
                    let list_link = document.createElement("a");
                    ulBlogsSearch.appendChild(list_item);
                    list_item.appendChild(list_link);
                    list_link.setAttribute('href', post.link);
                    if (!!post.thumbnail) {
                        let imgSearch = document.createElement("img");
                        imgSearch.setAttribute('src', post.thumbnail);
                        imgSearch.classList.add('blogs')
                        list_link.appendChild(imgSearch);
                    }
                    let contentName = document.createElement("span");
                    contentName.innerHTML = post.title;
                    list_link.appendChild(contentName);
                });
                document.getElementById('search-all-blog').setAttribute('href', "https://saymandigital.com/" + '?s=' + encodeURI(text));
            })
    }

};
SSM.init();

iconSearchFocus.addEventListener("click", function () {
    window.setTimeout(() => searchBoxInput.focus(), 0);
});

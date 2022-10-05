const commentsWrapper = document.querySelector(".comments-wrapper");
const productId = document.querySelector('.comments-wrapper').getAttribute('data-product');
const checkUserAuthenticated = document.querySelector('input[name="check_user_authenticated"]').value;
const commentChildTemplate = document.querySelector('.comment-child-template');
const commentsSort = document.querySelectorAll('.comments-sort');
let sortValue = document.querySelector('.comments-sort').value;
const paginationWrapper = document.querySelector('.pagination-wrapper');

function renderCommentStars(rating) {
    let output = `<div className="d-inline-block star-rating">`;
    for (let i = 1; i <= 5; i++) {
        if (i <= Math.round(rating) ?? 3) {
            output += `<i class="icon fa-star"><svg xmlns="http://www.w3.org/2000/svg" width="18.446" height="17.522" viewBox="0 0 18.446 17.522"><path id="Icon_material-star" data-name="Icon material-star" d="M11,15.216,15.944,18.2l-1.312-5.624L19,8.792,13.248,8.3,11,3,8.752,8.3,3,8.792l4.368,3.784L6.056,18.2Z" transform="translate(-1.777 -1.719)" stroke-width="1"></path></svg></i>`;
        } else {
            output += `<i class="icon"><svg xmlns="http://www.w3.org/2000/svg" width="18.446" height="17.522" viewBox="0 0 18.446 17.522"><path id="Icon_material-star" data-name="Icon material-star" d="M11,15.216,15.944,18.2l-1.312-5.624L19,8.792,13.248,8.3,11,3,8.752,8.3,3,8.792l4.368,3.784L6.056,18.2Z" transform="translate(-1.777 -1.719)" stroke-width="1"></path></svg></i>`;
        }
    }
    output += `</div>`;
    return output;
}

async function handleLikeComment(element, type) {
    let commentId = element.parentElement.parentElement.parentElement.getAttribute('data-comment');
    let likesCount = element.parentElement.querySelector('.likes-count');
    let dislikesCount = element.parentElement.querySelector('.dislikes-count');
   const request = await fetch('/comments/' + commentId + '/set-type',
        {
            method: 'post',
            body: JSON.stringify({type: type}),
            headers: jsonFormHeaders,
        });

    const result = await request.json();
    if (request.status === 200) {
        likesCount.innerHTML = likeCountDisplay(result.likes_count);
        dislikesCount.innerHTML = likeCountDisplay(result.dislikes_count);
    } else if (result.code === 422) {
        for (const error of result.errors_list) {
            alert(error);
        }
    }
}

async function handleReplyComment(item) {
    let parentId = item.parentElement.querySelector('input[name="parent_id"]').value;
    let content = item.parentElement.querySelector('input[name="content"]');
    let messageBox = item.parentElement.querySelector('.response-message');
    if (!content.value.trim()) {
        content.parentElement.reportValidity();
        return;
    }
   const request = await fetch('/products/' + productId + '/comments',
        {
            method: 'post',
            body: JSON.stringify({content: content.value, parent_id: parentId}),
            headers: jsonFormHeaders,
        });

    const result = await request.json();
    if (request.status === 200) {
        content.value = '';
        content.innerHTML = '';
        messageBox.parentElement.classList.remove('d-none');
        messageBox.innerHTML = result.message;
    } else if (result.code === 422) {
        for (const error of result.errors_list) {
            alert(error);
        }
    }
}

function handleCommentActions() {
    // reply comments section
    const sendCommentBox = document.querySelectorAll(".send-comment-box");
    const commentReply = document.querySelectorAll(".reply-comment");
    commentReply.forEach(item => {
        item.addEventListener('click', () => {
            sendCommentBox.forEach(item => {
                item.classList.add('d-none');
                item.querySelector('.response-message').parentElement.classList.add('d-none');
                item.querySelector('.response-message').innerHtml = '';
            });
            item.parentElement.nextElementSibling.classList.remove('d-none');
            item.parentElement.nextElementSibling.querySelector('input[name="parent_id"]').value = item.parentElement.getAttribute('data-comment');
            item.parentElement.nextElementSibling.querySelector('input[name="content"]').focus();
        });
    });
    const commentReplyButton = document.querySelectorAll(".submit-reply");
    commentReplyButton.forEach(item => {
        item.addEventListener('click', async (event) => {
            event.preventDefault();
            item.disabled = true;
            await handleReplyComment(item);
            item.disabled = false;
        });
    });

    // like comments section
    const commentLikeButton = document.querySelectorAll(".like-comment");
    const commentDislikeButton = document.querySelectorAll(".dislike-comment");
    Array.from(commentLikeButton).forEach(function (element) {
        element.addEventListener('click', function () {
            handleLikeComment(element, 1);
        });
    });

    // dislike comments section
    Array.from(commentDislikeButton).forEach(function (element) {
        element.addEventListener('click', function () {
            handleLikeComment(element, 2);
        });
    });
}

function renderUsername(comment) {
    if(comment.is_agent){
        return '';
    }
    return comment.name ? comment.name:'ناشناس';
}

function renderCommentChildes(comment, commentContainer) {
    comment.children.forEach(function (comment) {
        let commentTemplate = commentChildTemplate.cloneNode(true);
        let username = commentTemplate.querySelector('.username');
        let createdAt = commentTemplate.querySelector('.created-at');
        let likesCount = commentTemplate.querySelector('.likes-count');
        let dislikesCount = commentTemplate.querySelector('.dislikes-count');
        let content = commentTemplate.querySelector('.comment-content');
        let parentId = commentTemplate.querySelector('input[name="parent_id"]');
        let commentLikesBox = commentTemplate.querySelector('.comment-likes-box');
        let replyCommentButton = commentTemplate.querySelector('.reply-comment');
        let scoreComment = commentTemplate.querySelector('.score-comment');
        let avatar = commentTemplate.querySelector('.avatar');

        comment.image ? avatar.setAttribute('src', comment.image) : '';
        username.innerHTML = renderUsername(comment);
        createdAt.innerHTML = comment.created_at;
        likesCount.innerHTML = likeCountDisplay(comment.likes);
        dislikesCount.innerHTML = likeCountDisplay(comment.dislikes);
        content.innerHTML = comment.content;
        parentId.value = comment.id;
        scoreComment.setAttribute('data-comment', comment.id);

        if (!checkUserAuthenticated) {
            commentLikesBox.classList.add('d-none');
            replyCommentButton.classList.add('d-none');
        }
        commentTemplate.classList.remove("comment-child-template", "d-none");

        commentTemplate.setAttribute('id', 'comment-' + comment.id);

        if(comment.is_agent){
            commentTemplate.classList.add('sayman-comment');
        }
        commentContainer.appendChild(commentTemplate);
        if (comment.children) {
            renderCommentChildes(comment, commentContainer);
        }
    });
}

function handleCommentsPagination(paginationHtml) {
    document.querySelector('.pagination-holder').innerHTML = paginationHtml;
    let paginationLinks = document.querySelectorAll('.pagination-holder .pagination li a');
    paginationLinks.forEach(item => {
        item.addEventListener('click', (event) => {
            event.preventDefault();
            if (!!item.getAttribute('href') && item.getAttribute('href') !== 'null') {
                let url = new URL(item.getAttribute('href'));
                getComments(sortValue, url.searchParams.get("page"), true);
            }
        });
    });
}

async function getComments(sort = "date", pageNumber = 1, scroll = false) {
    let comments = document.querySelectorAll('.comments');
    comments.forEach(item => {
        item.remove();
    });
    let productId = commentsWrapper.getAttribute('data-product');
    await fetch('/products/' + productId + '/comments?sort=' + sort + '&page_number=' + pageNumber,)
        .then((response) => {
            if (response.ok) {
                return response.json();
            }
        }).then((response) => {
            if (response) {
                let comments = response.comments.data;

                let commentsContainer = document.querySelector('.comments-container');
                const commentsLength = comments.length;
                comments.forEach(function (comment, i) {
                    let commentTemplate = document.querySelector('.comment-template');
                    commentTemplate = commentTemplate.cloneNode(true);
                    let avatar = commentTemplate.querySelector('.avatar');
                    let username = commentTemplate.querySelector('.username');
                    let createdAt = commentTemplate.querySelector('.created-at');
                    let likesCount = commentTemplate.querySelector('.likes-count');
                    let dislikesCount = commentTemplate.querySelector('.dislikes-count');
                    let content = commentTemplate.querySelector('.comment-content');
                    let parentId = commentTemplate.querySelector('input[name="parent_id"]');
                    let recommend = commentTemplate.querySelector('.ok-product .recommend');
                    let notRecommend = commentTemplate.querySelector('.not-recommend');
                    let scoreComment = commentTemplate.querySelector('.score-comment');
                    let commentLikesBox = commentTemplate.querySelector('.comment-likes-box');
                    let starRating = commentTemplate.querySelectorAll('.star-rating');
                    let replyCommentButton = commentTemplate.querySelector('.reply-comment');
                    const pluck = key => array => Array.from(new Set(array.map(obj => obj[key])));
                    const getIds = pluck('id');

                    comment.image ? avatar.setAttribute('src', comment.image) : '';
                    username.innerHTML = renderUsername(comment);
                    createdAt.innerHTML = comment.created_at;
                    likesCount.innerHTML = likeCountDisplay(comment.likes);
                    dislikesCount.innerHTML = likeCountDisplay(comment.dislikes);
                    const stars = renderCommentStars(comment.rating);
                    starRating.forEach(item => {
                        item.innerHTML = stars;
                    });
                    content.innerHTML = comment.content;
                    parentId.value = comment.id;
                    scoreComment.setAttribute('data-comment', comment.id);
                    // if (getIds(product.not_recommended_by).includes(comment.user_id)) {
                    //     notRecommend.classList.remove('d-none');
                    // } else if (getIds(product.recommended_by).includes(comment.user_id)) {
                    //     recommend.classList.remove('d-none');
                    // }
                    if (!checkUserAuthenticated) {
                        commentLikesBox.classList.add('d-none');
                        replyCommentButton.classList.add('d-none');
                    }
                    // if (i === commentsLength - 1) { // Find Last Item Of The Array
                    //     commentTemplate.classList.remove("advanced-comment");
                    //     commentTemplate.classList.add("other-single-comment");
                    // }
                    commentTemplate.classList.remove("comment-template", "d-none");
                    commentTemplate.classList.add('comments');
                    if(comment.is_agent){
                        commentTemplate.classList.add('sayman-comment');
                    }
                    commentsContainer.appendChild(commentTemplate);
                    renderCommentChildes(comment, commentTemplate);
                });
                handleCommentsPagination(response.pagination);
                handleCommentActions();
                if(scroll){
                    document.querySelector('a[href="#comments-scroll"]').click();
                }
            }
        });
}

function likeCountDisplay(count)
{
    if(count == 0){
        return '';
    }
    return '(' + Intl.NumberFormat('fa-IR').format(count) + ')';
}

window.addEventListener("load", function () {
    getComments('date', 1, false);
});

commentsSort.forEach(item => {
    item.onchange = async function () {
        sortValue = item.value;
        await getComments(sortValue, 1, true);
    };
});



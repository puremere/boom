const addCommentButton = document.getElementById("add-comment-button");
const addCommentToggle = document.getElementById("add-comment-toggle");
const submitButton = document.getElementById("submit-comment-btn");
const buttonCloseAddCommentPage = document.getElementById("close-add-comment-page");
const ProductId = document.getElementById('product_id').value;
const content = document.getElementById('new-comment-content');
const recommend = document.querySelector('input[name="recommend"]');
const responseMessage = document.querySelector('.new-comment-response');
const handleClickAddComment = () => {
    addCommentToggle.classList.add("show");
    document.body.style.overflowY = "hidden";
}
let starRatingControl = new StarRating('.star-rating', {
    classNames: {
        active: 'gl-active',
        base: 'gl-star-rating',
        selected: 'gl-selected',
    },
    maxStars: 5,
    tooltip: false,
});
const handleCloseAddCommentPage = () => {
    document.body.style.overflowY = "auto";
    addCommentToggle.classList.remove("show");
    content.value = '';
    content.innerHTML = '';
    responseMessage.innerHTML = '';
    responseMessage.parentElement.classList.add('d-none');
};

buttonCloseAddCommentPage.addEventListener("click", handleCloseAddCommentPage);
const handleClickSubmit = async () => {
    let rating = starRatingControl.widgets[0].indexSelected + 1;
    const recommend = document.querySelector('input[name="recommend"]:checked');
    await fetch(`/products/${ProductId}/comments`,
        {
            method: 'post',
            body: JSON.stringify({
                content: content.value,
                rating,
                recommend: recommend ? recommend.value : ''
            }),
            headers: jsonFormHeaders,
        })
        .then((response) => {
            if (response.ok) {
                return response.json();
            }
            alert('متاسفانه مشکلی پیش آمده است، لطفا مجددا تلاش کنید!')
        }).then((response) => {
            if (response) {
                responseMessage.innerHTML = response.message;
                responseMessage.parentElement.classList.remove('d-none');
                setTimeout(function () {
                    handleCloseAddCommentPage();
                }, 1000);
            }
        });
}

submitButton.addEventListener("click", async function (e) {
    await new AddCommentValidator(e, '#add_new_comment_form');
    const errors = this.form.querySelectorAll('.validation-error');
    if (errors.length === 0) {
        submitButton.disabled = true;
        await handleClickSubmit();
        submitButton.disabled = false;
    }
});
if (addCommentButton) {
    addCommentButton.addEventListener("click", handleClickAddComment);
}

class AddCommentValidator {
    hasErrors = false;
    invalidElements = [];

    definedRules = {
        required: (input) => {
            if (input.value.trim() !== '') {
                return true;
            }
            this.hasErrors = true;
            this.invalidElements.push({
                input,
                message: "تکمیل این فیلد اجباری است."
            });
        },
        digits: (input, value) => {
            if (input.value.length === parseInt(value)) {
                return true;
            }
            this.hasErrors = true;
            this.invalidElements.unshift({
                input,
                message: "تعداد ارقام این فیلد باید برابر با " + value + " باشد."
            });
        }
    }

    constructor(e, form) {
        form = document.querySelector(form);
        this.form = form;
        this.hasErrors = false;
        this.invalidElements = [];
        this.removeErrors();
        const inputs = form.parentElement.querySelectorAll('[data-rules]');
        for (const input of inputs) {
            const givenRules = input.getAttribute('data-rules').split('|');
            for (const givenRule of givenRules) {
                let rule = givenRule;
                let args = null;
                if (givenRule.indexOf(':') > -1) {
                    const splittedRule = givenRule.split(':');
                    rule = splittedRule[0];
                    args = splittedRule[1];
                }
                if (this.definedRules.hasOwnProperty(rule)) {
                    this.definedRules[rule](input, args);
                }
            }
        }
        if (this.hasErrors) {
            e.stopPropagation();
            e.preventDefault();
            for (const error of this.invalidElements) {
                const message = document.createElement('p');
                message.classList.add('validation-error');
                message.innerHTML = error.message;

                const input = error.input;
                input.parentNode.insertBefore(message, input.nextSibling);

                const icon = message.parentElement.querySelector('.error-icon');
                icon.classList.add('d-block');
            }
        }
    }

    removeErrors() {
        const errors = this.form.querySelectorAll('.validation-error');
        errors.forEach(error => {
            error.remove();
        });
        const errorIcons = this.form.querySelectorAll('.error-icon');
        errorIcons.forEach(error => {
            error.classList.remove('d-block');
        });
    }
}

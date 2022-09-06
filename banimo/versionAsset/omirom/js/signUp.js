window.codeSentAt = []

const inputPhoneLoginPage = document.getElementById('input-phone-login-page');
const submitFormNumberLoginPage = document.getElementById('submit-form-number-login-page');
const phoneNumberLoginPage = document.getElementById('phone-login-page');
const sendCodeLoginPage = document.getElementById('send-code-login-page');
const defaultPhoneLoginPage = document.getElementById('default-phone-login-page');
const changePhoneNumberLogin = document.getElementById('change-phone-login-page');
const inputCodeLogin = [...document.querySelectorAll('input.code-input-login-page')];
const firstBoxLoginCode = document.getElementById('first-box-code');
const parentInputCodeLogin = document.getElementById('parent-input-code-login');
const getCodeLoginPage = document.getElementById('get-code-login-page');
const timerSendLoginPage = document.getElementById('timer-send-code-login-page');
const goLoginPasswordPage = document.getElementById('go-login-password-page');
const loginPassword = document.getElementById('login-password');
const goResetPasswordPage = document.getElementById('reset-password-page');
const goCodePageLogin = document.getElementById('go-code-page');
const inputPasswordLoginPage = document.getElementById('input-password-login-page');

const registerInformation = document.getElementById('register-information')
const registerInformationForm = document.getElementById('form-register-information')
const goPhonePage = document.getElementById('change-button-code-log-in')
const forgottenPassword = document.getElementById('forgotten-password');
const forgottenPasswordForm = document.getElementById('form-forgotten-password');

const phoneNumberReg = new RegExp('^(\\+98|0)?9\\d{9}$');
let numberPhone;

const resetButtonSendCodeLoginPage = () => {
    getCodeLoginPage.disabled = false;
    getCodeLoginPage.textContent = 'ارسال مجدد کد'
    getCodeLoginPage.classList.remove('start');
    getCodeLoginPage.classList.add('end');
}
const startButtonSendCodeLoginPage = (timeCode = 0) => {
    getCodeLoginPage.disabled = true;
    getCodeLoginPage.textContent = 'ارسال مجدد کد تا' + ' ' + timeCode + ' ' + 'دیگر';
    getCodeLoginPage.classList.add('start');
    getCodeLoginPage.classList.remove('end');
}

let clearTimerLoginPage = false;
const timerSendCodeLoginPage = () => {
    const maxTime = 120000;
    let min = maxTime - Math.abs(Date.parse(window.codeSentAt[window.phone]) - Date.now())
    if (min <= 0)
        min = 0

    // console.log('phone: ' + window.phone + ', at: ' + window.codeSentAt[window.phone] + ', ms: ' + min)
    let timer = setInterval(function () {
        let minutesCode = Math.floor((min % (1000 * 60 * 60)) / (1000 * 60));
        let secondsCode = Math.floor((min % (1000 * 60)) / 1000);
        min = min - 1000;
        if (minutesCode < 10) {
            minutesCode = '0' + minutesCode;
        }
        if (secondsCode < 10) {
            secondsCode = '0' + secondsCode;
        }
        let timeCode = minutesCode.toLocaleString().toPersianDigits() + ':' + secondsCode.toLocaleString().toPersianDigits();
        timerSendLoginPage.innerHTML = timeCode;
        startButtonSendCodeLoginPage(timeCode);
        if (min < 0 || clearTimerLoginPage == true) {
            resetButtonSendCodeLoginPage();
            clearInterval(timer);
        }
    }, 1000);
}

function showErrorInPhonePage(msg) {
    phoneNumberLoginPage.classList.remove('remove-page');
    inputPhoneLoginPage.classList.add('error-phone');
    document.getElementById('text-error-phone').innerHTML = msg
}

function showUnusualErrorInPhonePage(state) {
    document.getElementById('response_box_' + state).classList.remove('d-none')
    document.getElementById('showing_response_' + state).innerHTML = 'مجددا تلاش کنید.'
    document.getElementById('showing_response_' + state).classList.remove('alert-success')
    document.getElementById('showing_response_' + state).classList.add('alert-danger')
}

function actionAfterCheckPhone(data) {
    numberPhone = inputPhoneLoginPage.value.toLocaleString().toPersianDigits()
    defaultPhoneLoginPage.value = numberPhone
    inputPhoneLoginPage.classList.remove('error-phone')
    phoneNumberLoginPage.classList.add('remove-page')
    sendCodeLoginPage.classList.add('add-page')

    if (data.isVerified) {
        window.station = 'login'
        goLoginPasswordPage.classList.remove('d-none')
    } else {
        window.station = 'register'
        goLoginPasswordPage.classList.add('d-none')
    }

    firstBoxLoginCode.focus()
    clearTimerLoginPage = false
    timerSendCodeLoginPage()
}

inputPhoneLoginPage.addEventListener('input', function (e) {
    if (e.target.value.length > e.target.maxLength) e.target.value = e.target.value.slice(0, e.target.maxLength);
});

changePhoneNumberLogin.addEventListener('click', function () {
    clearTimerLoginPage = true;
    phoneNumberLoginPage.classList.remove('remove-page');
    document.getElementById('text-error-phone').innerHTML = ''
    sendCodeLoginPage.classList.remove('add-page');
    inputPhoneLoginPage.focus();
});

/*click button numberPhone*/
submitFormNumberLoginPage.addEventListener('submit', function (e) {
    e.preventDefault();
    sendCheckPhone(e)
});

function sendCheckPhone() {
    if (inputPhoneLoginPage.value && !phoneNumberReg.test(inputPhoneLoginPage.value)) {
        showErrorInPhonePage('شماره موبایل نامعتبر است.')
        return;
    }
    window.phone = inputPhoneLoginPage.value
    fetch(baseURL.concat('auth/check/phone'), {
        body: JSON.stringify({
            phone: inputPhoneLoginPage.value,
        }),
        headers: {
            'X-CSRF-TOKEN': csrfToken,
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        method: 'POST'
    })
        .then(response => {
            return response.json()
        })
        .then(data => {
            document.getElementById('text-error-phone').innerHTML = ''
            if (data.code === 200) {
                window.codeSentAt[window.phone] = data.code_sent_at
                if (data.status_message === 'VerificationCodeSend' || data.status_message === 'OtpPasswordSend') {
                    actionAfterCheckPhone(data)
                    document.getElementById('error-in-code-page').innerHTML = ''
                } else if (data.status_message === 'VerificationCodeHasBeenSent' || data.status_message === 'OtpPasswordHasBeenSent') {
                    actionAfterCheckPhone(data)
                    document.getElementById('error-in-code-page').innerHTML = data.message
                } else {
                    showErrorInPhonePage(data.message)
                }
            } else if (data.code === 422) {
                showErrorInPhonePage(data.errors['phone'].join('<br>'))
            } else {
                showUnusualErrorInPhonePage('phone_number')
            }
        });
}

/*inputs code phone*/
inputCodeLogin.forEach((ele, index) => {
    inputCodeLogin[0].addEventListener('keydown', function (event) {
        if (event.keyCode == 8) {
            inputCodeLogin[0].value = '';
        }
    })
    ele.addEventListener('keydown', (e) => {
        if (e.keyCode === 8 && e.target.value === '') inputCodeLogin[Math.max(0, index - 1)].focus()
    })
    ele.addEventListener('input', (e) => {
        const [first, ...rest] = e.target.value
        e.target.value = first ?? ''
        if (index !== inputCodeLogin.length - 1 && first !== undefined && inputCodeLogin[index + 1].value == '') {
            inputCodeLogin[index + 1].focus()
            inputCodeLogin[index + 1].value = rest.join('')
            inputCodeLogin[index + 1].dispatchEvent(new Event('input'));
            inputCodeLogin[index + 1].addEventListener('keydown', function (event) {
                if (event.keyCode == 8) {
                    inputCodeLogin[index + 1].value = '';
                }
            })
        }
        const code = [...document.querySelectorAll('input.code-input-login-page')]
            .filter(({name}) => name)
            .map(({value}) => value)
            .join('')
        if (code.length === 5) {
            //action
            window.code = code
            window.phone = inputPhoneLoginPage.value
            switch (window.station) {
                case 'register':
                    fetch(baseURL.concat('auth/check/verification-code'), {
                        body: JSON.stringify({
                            phone: window.phone,
                            verification_code: window.code,
                        }),
                        headers: {
                            'X-CSRF-TOKEN': csrfToken,
                            'Content-Type': 'application/json',
                            'Accept': 'application/json',
                        },
                        method: 'POST'
                    })
                        .then(response => {
                            return response.json()
                        })
                        .then(data => {
                            if (data.code === 200) {
                                if (data.status_message === 'VerificationCodeValid') {
                                    parentInputCodeLogin.classList.remove('error-code');
                                    sendCodeLoginPage.classList.remove('add-page');
                                    registerInformation.classList.add('add-page');
                                    registerInformation.classList.remove('d-none');
                                } else {
                                    document.getElementById('error-in-code-page').innerHTML = data.message
                                }
                            } else if (data.code === 422) {
                                parentInputCodeLogin.classList.add('error-code');
                            } else {
                                showUnusualErrorInPhonePage('code')
                            }
                        })
                    break
                case 'login':
                    fetch(baseURL.concat('auth/check/otp-password'), {
                        body: JSON.stringify({
                            phone: window.phone,
                            otp_password: window.code,
                        }),
                        headers: {
                            'X-CSRF-TOKEN': csrfToken,
                            'Content-Type': 'application/json',
                            'Accept': 'application/json',
                        },
                        method: 'POST'
                    })
                        .then(response => {
                            return response.json()
                        })
                        .then(data => {
                            if (data.code === 200) {
                                if (data.status_message === 'OtpPasswordValid') {
                                    window.location.replace(typeof data.redirect == 'string' ? data.redirect:'');
                                } else {
                                    document.getElementById('error-in-code-page').innerHTML = data.message
                                }
                            } else if (data.code === 422) {
                                parentInputCodeLogin.classList.add('error-code');
                            } else {
                                showUnusualErrorInPhonePage('code')
                            }
                        })
                    break
                case 'password-recovery':
                    fetch(baseURL.concat('auth/check/password-recovery'), {
                        body: JSON.stringify({
                            phone: window.phone,
                            verification_code: window.code,
                        }),
                        headers: {
                            'X-CSRF-TOKEN': csrfToken,
                            'Content-Type': 'application/json',
                            'Accept': 'application/json',
                        },
                        method: 'POST'
                    })
                        .then(response => {
                            return response.json()
                        })
                        .then(data => {
                            if (data.code === 200) {
                                if (data.status_message === 'RecoveryPasswordValid') {
                                    parentInputCodeLogin.classList.remove('error-code');
                                    sendCodeLoginPage.classList.remove('add-page');
                                    forgottenPassword.classList.add('add-page')
                                    forgottenPassword.classList.remove('d-none')
                                    document.getElementById('input-new-password').focus()
                                } else {
                                    document.getElementById('error-in-code-page').innerHTML = data.message
                                }
                            } else if (data.code === 422) {
                                parentInputCodeLogin.classList.add('error-code');
                            } else {
                                showUnusualErrorInPhonePage('code')
                            }
                        })
                    break
            }
        } else {
            parentInputCodeLogin.classList.remove('error-code');
        }
    })
});

registerInformationForm.addEventListener('submit', function (e) {
    e.preventDefault();
    fetch(baseURL.concat('auth/store/information'), {
        body: JSON.stringify({
            phone: window.phone,
            verification_code: window.code,
            first_name: document.getElementById('first_name').value,
            last_name: document.getElementById('last_name').value,
            password: document.getElementById('password').value,
        }),
        headers: {
            'X-CSRF-TOKEN': csrfToken,
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        method: 'POST'
    })
        .then(response => {
            return response.json()
        })
        .then(data => {
            document.querySelector('.response-error').innerHTML = ''
            if (data.code === 200) {
                if (data.status_message === 'RegisterUserSuccess') {
                    window.location.replace(baseURL);
                } else {
                    document.getElementById('error-in-register-page').innerHTML = data.message
                }
            } else if (data.code === 422) {
                Object.keys(data.errors).forEach(function (key) {
                    document.querySelector('#' + key).parentElement
                        .querySelector('.response-error').innerHTML = data.errors[key].join('<br>')
                })
            } else {
                showUnusualErrorInPhonePage('register_info')
            }
        })
})

document.getElementById('login-password-btn').addEventListener('click', function (e) {
    e.preventDefault();
    document.getElementById('error-in-login-password-page').innerHTML = ''
    fetch(baseURL.concat('auth/check/password'), {
        body: JSON.stringify({
            phone: window.phone,
            verification_code: window.code,
            password: document.getElementById('input-password-login-page').value,
        }),
        headers: {
            'X-CSRF-TOKEN': csrfToken,
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        method: 'POST'
    })
        .then(response => {
            return response.json()
        })
        .then(data => {
            document.getElementById('error-in-login-password-page').innerHTML = ''
            if (data.code === 200) {
                if (data.status_message === 'PasswordValid') {
                    window.location.replace(baseURL);
                } else {
                    document.getElementById('error-in-login-password-page').innerHTML = data.message
                }
            } else if (data.code === 401) {
                document.getElementById('error-in-login-password-page').innerHTML = data.message
            } else if (data.code === 422) {
                Object.keys(data.errors).forEach(function (key) {
                    let tmp = data.errors[key].join('<br>')
                    tmp += '<br>'
                    document.getElementById('error-in-login-password-page').innerHTML += tmp
                })
            } else {
                showUnusualErrorInPhonePage('login_password')
            }
        })
})

getCodeLoginPage.addEventListener('click', function () {
    clearTimerLoginPage = false;
    startButtonSendCodeLoginPage();
    timerSendCodeLoginPage();

    if (window.station === 'password-recovery') {
        sendPasswordRecovery()
    } else {
        sendCheckPhone()
    }
});

goLoginPasswordPage.addEventListener('click', function () {
    actionBeforeOpenLoginPasswordPage()
})

function actionBeforeOpenLoginPasswordPage() {
    clearTimerLoginPage = true
    sendCodeLoginPage.classList.remove('add-page')
    forgottenPassword.classList.remove('add-pge')
    forgottenPassword.classList.add('d-none')

    loginPassword.classList.add('add-page')
    document.getElementById('input-phone-email-login').value = window.phone.toPersianDigits()
    document.getElementById('error-in-login-password-page').innerHTML = ''
    inputPasswordLoginPage.focus();
}

goPhonePage.addEventListener('click', function () {
    clearTimerLoginPage = true
    loginPassword.classList.remove('add-page')
    phoneNumberLoginPage.classList.remove('remove-page')
    inputPhoneLoginPage.focus()
})

goCodePageLogin.addEventListener('click', function () {
    actionBeforeOpenCodePage()
})

function actionBeforeOpenCodePage() {
    clearTimerLoginPage = false;
    startButtonSendCodeLoginPage();
    timerSendCodeLoginPage();
    sendCodeLoginPage.classList.add('add-page');
    loginPassword.classList.remove('add-page');
    document.getElementById('error-in-code-page').innerHTML = ''
    firstBoxLoginCode.focus();
}

goResetPasswordPage.addEventListener('click', function (e) {
    e.preventDefault()
    sendPasswordRecovery()
})

function sendPasswordRecovery() {
    window.station = 'password-recovery'
    fetch(baseURL.concat('auth/send/password-recovery'), {
        body: JSON.stringify({
            phone: window.phone,
        }),
        headers: {
            'X-CSRF-TOKEN': csrfToken,
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        method: 'POST'
    })
        .then(response => {
            return response.json()
        })
        .then(data => {
            if (data.code === 200) {
                window.codeSentAt[window.phone] = data.code_sent_at
                if (data.status_message === 'RecoveryPasswordSend') {
                    actionBeforeOpenCodePage()
                } else {
                    document.getElementById('error-in-login-password-page').innerHTML = data.message
                }
            } else if (data.code === 422) {
                document.getElementById('error-in-login-password-page').innerHTML = data.errors['phone'].join('<br>')
            } else {
                showUnusualErrorInPhonePage('login_password')
            }
        })
}

forgottenPasswordForm.addEventListener('submit', function (e) {
    e.preventDefault()
    document.getElementById('error-in-forgotten-password-page').innerHTML = ''
    fetch(baseURL.concat('auth/store/new-password'), {
        body: JSON.stringify({
            phone: window.phone,
            verification_code: window.code,
            password: document.getElementById('input-new-password').value,
            confirmed_password: document.getElementById('input-new-password-confirmed').value,
        }),
        headers: {
            'X-CSRF-TOKEN': csrfToken,
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        method: 'POST'
    })
        .then(response => {
            return response.json()
        })
        .then(data => {
            document.getElementById('error-in-forgotten-password-page').innerHTML = ''
            if (data.code === 200) {
                if (data.status_message === 'StoreNewPasswordSuccess') {
                    // actionBeforeOpenLoginPasswordPage()
                    location.reload();
                } else {
                    document.getElementById('error-in-forgotten-password-page').innerHTML = data.message
                }
            } else if (data.code === 422) {
                Object.keys(data.errors).forEach(function (key) {
                    let tmp = data.errors[key].join('<br>')
                    tmp += '<br>'
                    document.getElementById('error-in-forgotten-password-page').innerHTML += tmp
                })
            } else {
                showUnusualErrorInPhonePage('forgotten_password')
            }
        })
});

function toggleShowPassword(el) {
    let passwordInput = document.getElementById(el);
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
    } else {
        passwordInput.type = "password";
    }
}


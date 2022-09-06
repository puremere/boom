const timerInterval = document.querySelectorAll(".timer-interval-box");


timerInterval.forEach(item=>{
    const timerAttr = item.getAttribute("data-time-offer");
    const countDownData = new Date(timerAttr).getTime();
    let x = setInterval(function () {


        let now = new Date().getTime();


        let distanceOffer = countDownData - now;


        let hoursOffer = Math.floor(distanceOffer / (1000 * 60 * 60));
        let minutesOffer = Math.floor((distanceOffer % (1000 * 60 * 60)) / (1000 * 60));
        let secondsOffer = Math.floor((distanceOffer % (1000 * 60)) / 1000);

        if (hoursOffer < 10) {
            hoursOffer = "0" + hoursOffer;
        }
        if (minutesOffer < 10) {
            minutesOffer = "0" + minutesOffer;
        }
        if (secondsOffer < 10) {
            secondsOffer = "0" + secondsOffer;
        }
        item.innerHTML = hoursOffer.toLocaleString().toPersianDigits() + " : "
            + minutesOffer.toLocaleString().toPersianDigits() + " : " + secondsOffer.toLocaleString().toPersianDigits();


        if (distanceOffer < 0) {
            clearInterval(x);
            item.innerHTML = "تمام شد";
        }

    }, 1000);

});

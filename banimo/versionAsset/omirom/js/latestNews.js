function toPersianDigits(n) {
    const farsiDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
    return n
        .toString()
        .split('')
        .map(x => farsiDigits[x])
        .join('');
}

function howLongSpend(today, releaseDate) {
    releaseDate = new Date(releaseDate);
    let Difference_In_Second = (today.getTime() - releaseDate.getTime()) / 1000;
    let min_ = Difference_In_Second / 60;
    let hour_ = min_ / 60;
    let day_ = hour_ / 24;

    let result;
    if (Math.floor(day_) !== 0)
        result = toPersianDigits(Math.floor(day_)) + ' روز پیش';
    else if (Math.floor(hour_) !== 0)
        result = toPersianDigits(Math.floor(hour_)) + ' ساعت پیش';
    else if (Math.floor(min_) !== 0)
        result = toPersianDigits(Math.floor(hour_)) + ' دقیقه پیش';
    else
        result = 'لحظاتی پیش';

    return result;
}

$(document).ready(function () {
    const truncate = (str, max, suffix) => str.length < max ? str : `${str.substr(0, str.substr(0, max - suffix.length).lastIndexOf(' '))}${suffix}`;
    $('#latestNewsBox').hide();
    $.get("/api/blog/posts", function (data, status) {
        if (status === 'success') {
            $('#latestNewsBox').show();
            let i;
            let today = new Date();
            for (i = 0; i < data.length; i++) {
                $('#lnb_slide_'.concat(i)).show();
                $('#lnb_url_'.concat(i)).attr('href', data[i]['link']);
                $('#lnb_img_'.concat(i)).attr('src', data[i]['image_md']);
                $('#lnb_img_'.concat(i)).attr('alt', data[i]['title']);
                $('#lnb_title_'.concat(i)).text(truncate(data[i]['title'], 61, ''));
                $('#lnb_excerpt_'.concat(i)).text(truncate(data[i]['excerpt'], 155, ' ...'));
                $('#lnb_avatar_'.concat(i)).attr('src', data[i]['author']['avatar']);
                $('#lnb_avatar_'.concat(i)).attr('alt', data[i]['author']['name']);
                $('#lnb_author_name_'.concat(i)).text(data[i]['author']['name']);
                $('#lnb_date_'.concat(i)).text(howLongSpend(today, data[i]['date']));

                $('#lnb_m_slide_'.concat(i)).show();
                $('#lnb_m_url_'.concat(i)).attr('href', data[i]['link']);
                $('#lnb_m_img_'.concat(i)).attr('src', data[i]['image_md']);
                $('#lnb_m_img_'.concat(i)).attr('alt', data[i]['title']);
                $('#lnb_m_title_'.concat(i)).text(truncate(data[i]['title'], 61, ''));
            }
        } else
            console.error('Latest News: can not get request');
    });
});



// locations-section
// select-package-type
// shipping-type
// contact-info

$(document).ready(function(){
    prepareForm();
});

$('.previous').click(function () {
    let target = $(this).attr('id');

    switch (target) {
       

        case 'step2':
            $("#locations-section , #select-package-type, #shipping-type, #contact-info, #account-info").addClass('d-none');
            $('#locations-section').removeClass('d-none');
            goReadPoint();
            break;

        case 'step3':
            $("#locations-section , #select-package-type, #shipping-type, #contact-info, #account-info").addClass('d-none');
            $('#select-package-type').removeClass('d-none');
            goReadPoint();
            break;


        case 'step4':
            $("#locations-section , #select-package-type, #shipping-type, #contact-info, #account-info").addClass('d-none');
            $('#shipping-type').removeClass('d-none');
            goReadPoint();
            break;

        case 'step5':
             $("#locations-section , #select-package-type, #shipping-type, #contact-info, #account-info").addClass('d-none');
            $('#contact-info').removeClass('d-none');

            //Show Sammary
            //$("#info-sec").addClass('d-none');
            //$("#summary-sec").removeClass('d-none');
            goReadPoint();
            break;

    }

});

$('.nexts').click(function(){
    let target = $(this).attr('id');

    switch(target){
        case'step1':
            $("#locations-section , #select-package-type, #shipping-type, #contact-info, #account-info").addClass('d-none');
            $(' #select-package-type').removeClass('d-none');
            goReadPoint();
        break;

        case'step2':
            $("#locations-section , #select-package-type, #shipping-type, #contact-info, #account-info").addClass('d-none');
            $('#shipping-type').removeClass('d-none');
            goReadPoint();
        break;

        case'step3':
            $("#locations-section , #select-package-type, #shipping-type, #contact-info, #account-info").addClass('d-none');
            $('#contact-info').removeClass('d-none');
            goReadPoint();
        break;


        case'step4':
            $("#locations-section , #select-package-type, #shipping-type, #contact-info, #account-info").addClass('d-none');
            $('#account-info').removeClass('d-none');
            goReadPoint();
        break;

        case'step5':
            // $("#locations-section , #select-package-type, #shipping-type, #contact-info, #account-info").addClass('d-none');
            // $('#account-info').removeClass('d-none');

            //Show Sammary
            $("#info-sec").addClass('d-none');
            $("#summary-sec").removeClass('d-none');
            goReadPoint();
        break;

    }

});




function goReadPoint(){
    $('html, body').animate({
        scrollTop: $(".app-area").offset().top
    }, 200);
}

function prepareForm(){
    document.getElementById("date-input").valueAsDate = new Date()
    $(".time-input").val( "00:00" );

}


/** Vechal Cards Settings */
$('.VCards').click(function(){
    $('.VCards').removeClass('card-active');
    $(this).addClass('card-active');

    let value = $(this).attr('aria-label');
    let title = $(this).attr('id');

    $("#v-type").val(value);
    $("#v-type").attr('alt',title);
    refreshForm();

})



/** Delivery Type Settings */
$('.DayCards').click(function(){
    $('.DayCards').removeClass('card-active');
    $(this).addClass('card-active');

    let value = $(this).attr('aria-label');
    let title = $(this).attr('id');

    $("#day-type").val(value);
    $("#day-type").attr('alt',title);
    refreshForm();
})


$(".form-control, .form-select").on("change , keyup",function(){

    refreshForm();

});

function refreshForm(){
    let pickupLocation = $("#pickup-loc").val();
    $(".pickup-loc").html(pickupLocation);


    let senderName = $("#sender-name").val();
    $(".sender-name").html(senderName);
    $("#s-name").val(senderName);



    let senderPhone = $("#phone-no").val();
    $(".sender-phone").html(senderPhone);
    $("#ur-phone").val(pickupLocation);



    let senderAddress = $("#pickup-address").val();
    $(".sender-addr").html(senderAddress);

    // let packageType = $("#pack-type").val();
   


    let reachBy = $("#reching").val();
    $(".sender-addr").html(reachBy);

    let dropoffLoc = $("#dropoff-loc").val();
    $(".dropoff-loc").html(dropoffLoc);

    let reciverName = $("#r-name").val();
    $(".reciver-name").html(reciverName);

    let reciverPhone = $("#r-phone").val();
    $(".reciver-phone").html(reciverPhone);


    let reciverAddress = $("#r-address").val();
    $(".reciver-address").html(reciverAddress);

    let reciverReaching = $("#r-reching").val();
    $(".rec-reaching").html(reciverReaching);

    let vType = $("#v-type").attr('alt');
    $(".v-type").html(vType);


    let bookingType = $("#day-type").attr('alt');
    $(".booking-type").html(bookingType);

    let date = $("#input-date").val();
    let timeFrom = $("#pickup-time-from").val();
    let timeTo = $("#pickup-time-to").val();
    let fullDate = date+", "+timeFrom+" - "+timeTo;
    $(".pickup-full-date").html(fullDate);

    //.dropoff-full-date

}

/**
    

 */
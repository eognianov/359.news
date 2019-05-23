(function ($) {
   
    $notifications = $('.toast__close i').on('click', cookieHandler);
    // cookie
    function  cookieHandler(event){
        event.stopPropagation();
        $notification = $(this.parentNode.parentNode);
        if ($notification.siblings().filter(".hidden").length === $notification.siblings().length) {
            $('#toast__container').hide();
        }

        // if ($notification.siblings()===0) {
        //     $('#toast__container').hide();
        // }
        $notification.addClass("hidden");
    };
}    
)(jQuery);
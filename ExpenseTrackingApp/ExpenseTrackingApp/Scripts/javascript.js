function myFunction() {
    var $this = $(this);

    $('#fh5co-offcanvass').toggleClass('fh5co-awake');
    if ($(window).width() < 1600) {
        $('#fh5co-page, #fh5co-menu').toggleClass('fh5co-sleep');
    }

    if ($('#fh5co-offcanvass').hasClass('fh5co-awake')) {
        $this.addClass('active');
    } else {
        $this.removeClass('active');
    }
}
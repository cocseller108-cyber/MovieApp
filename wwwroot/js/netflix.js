// Simple animation when adding a movie to watchlist
$(document).ready(function () {
    $('.add-btn').on('click', function () {
        $(this).closest('.movie-card').fadeOut(300).fadeIn(500);
    });
});
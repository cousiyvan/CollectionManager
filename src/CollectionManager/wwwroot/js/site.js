// Write your Javascript code.

$(document).ready(function () {
    // Switch views of the elements to display in Games view
    // check which button is clicked and slide down/up it's content and update active class
    $(".GamesMenuItems").click(function () {
        if ($(this).attr("id") === "MyCollection")
        {
            if ($(this).hasClass("active")) {
                $(".MyCollectionGamesContainer").slideUp();
                $(this).removeClass("active");
            }
            else 
            {
                $(".MyCollectionGamesContainer").slideDown();
                $(this).addClass("active");
            }
        }
        else if ($(this).attr("id") === "MyWishlist") {
            if ($(this).hasClass("active")) {
                $(".MyWishlistGamesContainer").slideUp();
                $(this).removeClass("active");
            }
            else
            {
                $(".MyWishlistGamesContainer").slideDown();
                $(this).addClass("active");
            }
        }
        else if ($(this).attr("id") === "MyFavorites") {
            if ($(this).hasClass("active")) {
                $(".MyFavoritesGamesContainer").slideUp();
                $(this).removeClass("active");
            }
            else {
                $(".MyFavoritesGamesContainer").slideDown();
                $(this).addClass("active");
            }
        }
        else if ($(this).attr("id") === "LatestReleases") {
            if ($(this).hasClass("active")) {
                $(".MyLatestReleasesGamesContainer").slideUp();
                $(this).removeClass("active");
            }
            else {
                $(".MyLatestReleasesGamesContainer").slideDown();
                $(this).addClass("active");
            }
        }
    });

    function triggerAlert(alertMessage) {}
    // alert(alertMessage);
    if (alertMessage !== "")
    {
        $(".alert-container").fadeTo(2000, 500).slideUp(500, function() {
            $(".alert-container").slideUp(500);
        });
    }
});
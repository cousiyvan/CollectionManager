﻿// Write your Javascript code.

$(document).ready(function () {
    var baseUrl = {
        "games": "https://igdbcom-internet-game-database-v1.p.mashape.com/"
    };
    searchUrl = "";

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
        else if ($(this).attr("id") === "Search") {
            if ($(this).hasClass("active")) {
                $(".SearchGameContainer").slideUp();
                $(this).removeClass("active");
            }
            else {
                $(".SearchGameContainer").slideDown();
                $(this).addClass("active");
            }
        }
    });

    // Hide some components
    $("#ReleasesDateFilterComponents").hide();
    $("#SearchQueryDisplay").hide();
    $("#ratingFiltersComponents").hide();

    // Dealing with search filters
    $("#ReleaseDateFilter").click(function () {
        $("#ReleasesDateFilterComponents").show();
    });

    $("#ascendantFilter").click(function () {
        FillSearchQueryParameters("Releases Dates - ascendant filters");
        $("#SearchQueryDisplay").show();
        searchUrl = "games/?search=" + $("#input-search").val() + "&order=release_dates.date:asc";
    });
    $("#descendantFilter").click(function () {
        FillSearchQueryParameters("Releases Dates - descendant filters");
        $("#SearchQueryDisplay").show();
        searchUrl = "games/?search=" + $("#input-search").val() + "&order=release_dates.date:desc";
    });

    $("#SearchSubmit").click(function () {
        AddToCart(searchUrl);
    });

    function FillSearchQueryParameters(filters) {
        var inputSearch = $("#input-search").val();
        $("#SearchQuery").text(inputSearch + " - " + filters);
    }

    function AddToCart(searchQuery)
    {
        $.ajax({
            type: 'POST',
            url: '/Collection/SearchGames',
            data: { parameterQuery: searchQuery }
        }).done(function () {
            $(".MyLatestReleasesGamesContainer").html("");
            $(".MyLatestReleasesGamesContainer").load("/Collection/SearchGamesDisplay");
            alert("Done");
        });
    }

    function triggerAlert(alertMessage) {}
    // alert(alertMessage);
    if (alertMessage !== "") {
        $(".alert-container").fadeTo(2000, 500).slideUp(500, function () {
            $(".alert-container").slideUp(500);
        });
    }
});
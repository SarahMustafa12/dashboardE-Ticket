
//const arrows = document.querySelectorAll(".arrow");
//const movieLists = document.querySelectorAll(".movie-list");

//arrows.forEach((arrow, i) => {
//  const itemNumber = movieLists[i].querySelectorAll("img").length;
//  let clickCounter = 0;
//  arrow.addEventListener("click", () => {
//    const ratio = Math.floor(window.innerWidth / 270);
//    clickCounter++;
//    if (itemNumber - (4 + clickCounter) + (4 - ratio) >= 0) {
//      movieLists[i].style.transform = `translateX(${
//        movieLists[i].computedStyleMap().get("transform")[0].x.value - 300
//      }px)`;
//    } else {
//      movieLists[i].style.transform = "translateX(0)";
//      clickCounter = 0;
//    }
//  });

//  console.log(Math.floor(window.innerWidth / 270));
//});

////TOGGLE

//const ball = document.querySelector(".toggle-ball");
//const items = document.querySelectorAll(
//  ".container,.movie-list-title,.navbar-container,.sidebar,.left-menu-icon,.toggle"
//);

//ball.addEventListener("click", () => {
//  items.forEach((item) => {
//    item.classList.toggle("active");
//  });
//  ball.classList.toggle("active");
//});

//var $thumbs = $('#thumb-carousel');

//function slideshowInit() {
//  $thumbs.wrap('<div id="stage-wrap"></div>');
//  $('#stage-wrap').prepend('<div id="slideshow-next"></div><div id="slideshow-prev"></div><div id="stage"></div>');

//  var $stage = $('#stage');
//  $stage.css('opacity');
//  var $imageLinks = $thumbs.find('a');
//  var $src;

//  $imageLinks.each(function(i) {
//    $src = $(this).attr('href');
//    var $img = $('<img/>', { src: $src, css: { display: 'none' }, class: 'outdoorShadow' });
//    $img.appendTo($stage);
//  });

//  $stage.css('opacity', 1);

//  $imageLinks.bind('click', function() {
//    var index = $(this).data('index');
//    $(this).parents('li').addClass('current').siblings('.current').removeClass('current');

//    var nextImage = $stage.find('img:eq(' + index + ')');
//    $stage.find('img.active').fadeOut().removeClass('.active');
//    nextImage.fadeIn().addClass('active');
//    return false;
//  })
//  .filter(':first').click();
//}

//$(document).ready(function() {
//  if ($(window).width() > 500) {
//    $('div.toto').addClass('jcarousel-skin-tango').removeClass('noCarousel');
//    $('#thumb-carousel img').removeClass('outdoorShadow');

//    $('#thumb-carousel img').each(function() {
//      $(this).wrap('<a href="' + $(this).attr('src') + '"></a>');
//    });

//    $thumbs.find('a').each(function(index) {
//      $(this).data('index', index);
//    });

//    slideshowInit();

//    $thumbs.jcarousel({
//      vertical: true,
//      wrap: 'circular',
//      animation: 1500,
//      auto: 2
//    });
//  }
//});
//$(document).ready(function() {
//  // Auto-slide every 3 seconds
//  setInterval(function() {
//      var $carousel = $('#thumb-carousel');
//      var currentTransform = $carousel.css('transform');
//      var currentOffset = parseInt(currentTransform.split(',')[4], 10) || 0;

//      var newOffset = currentOffset - 200; // Assuming each item is 200px wide
//      $carousel.css('transform', 'translateX(' + newOffset + 'px)');
//  }, 3000); // 3000ms = 3 seconds
//});


//import { Dropdown, Collapse, initMDB } from "mdb-ui-kit";

//initMDB({ Dropdown, Collapse });

//let profile = document.querySelector('.profile');
//let menu = document.querySelector('.menu');

//profile.onclick = function () {
//    menu.classList.toggle('active');
//}

//$(document).ready(function () {
//    $.ajax({
//        url: '/EndUser/Cart/GetCartCount',
//        type: 'GET',
//        success: function (data) {
//            console.log("✅ Cart count received:", data);
//            $('#lblCartCount').text(data);
//        },
//        error: function (xhr, status, error) {
//            console.error("❌ AJAX error:", error);
//            console.log(xhr.responseText);
//        }
//    });
//});

// Slide arrows for movie list
const arrows = document.querySelectorAll(".arrow");
const movieLists = document.querySelectorAll(".movie-list");

arrows.forEach((arrow, i) => {
    const itemNumber = movieLists[i].querySelectorAll("img").length;
    let clickCounter = 0;
    arrow.addEventListener("click", () => {
        const ratio = Math.floor(window.innerWidth / 270);
        clickCounter++;
        if (itemNumber - (4 + clickCounter) + (4 - ratio) >= 0) {
            movieLists[i].style.transform = `translateX(${movieLists[i].computedStyleMap().get("transform")[0].x.value - 300
                }px)`;
        } else {
            movieLists[i].style.transform = "translateX(0)";
            clickCounter = 0;
        }
    });

    console.log(Math.floor(window.innerWidth / 270));
});

// Toggle dark/light mode
const ball = document.querySelector(".toggle-ball");
const items = document.querySelectorAll(
    ".container,.movie-list-title,.navbar-container,.sidebar,.left-menu-icon,.toggle"
);

ball.addEventListener("click", () => {
    items.forEach((item) => {
        item.classList.toggle("active");
    });
    ball.classList.toggle("active");
});

// Slideshow and thumbnail carousel
var $thumbs = $('#thumb-carousel');

function slideshowInit() {
    $thumbs.wrap('<div id="stage-wrap"></div>');
    $('#stage-wrap').prepend('<div id="slideshow-next"></div><div id="slideshow-prev"></div><div id="stage"></div>');

    var $stage = $('#stage');
    $stage.css('opacity');
    var $imageLinks = $thumbs.find('a');
    var $src;

    $imageLinks.each(function (i) {
        $src = $(this).attr('href');
        var $img = $('<img/>', { src: $src, css: { display: 'none' }, class: 'outdoorShadow' });
        $img.appendTo($stage);
    });

    $stage.css('opacity', 1);

    $imageLinks.bind('click', function () {
        var index = $(this).data('index');
        $(this).parents('li').addClass('current').siblings('.current').removeClass('current');

        var nextImage = $stage.find('img:eq(' + index + ')');
        $stage.find('img.active').fadeOut().removeClass('active');
        nextImage.fadeIn().addClass('active');
        return false;
    }).filter(':first').click();
}

$(document).ready(function () {
    if ($(window).width() > 500) {
        $('div.toto').addClass('jcarousel-skin-tango').removeClass('noCarousel');
        $('#thumb-carousel img').removeClass('outdoorShadow');

        $('#thumb-carousel img').each(function () {
            $(this).wrap('<a href="' + $(this).attr('src') + '"></a>');
        });

        $thumbs.find('a').each(function (index) {
            $(this).data('index', index);
        });

        slideshowInit();

        $thumbs.jcarousel({
            vertical: true,
            wrap: 'circular',
            animation: 1500,
            auto: 2
        });
    }
});

// Auto-slide carousel every 3 seconds
$(document).ready(function () {
    setInterval(function () {
        var $carousel = $('#thumb-carousel');
        var currentTransform = $carousel.css('transform');
        var currentOffset = parseInt(currentTransform.split(',')[4], 10) || 0;

        var newOffset = currentOffset - 200; // Assuming each item is 200px wide
        $carousel.css('transform', 'translateX(' + newOffset + 'px)');
    }, 3000);
});

// Profile dropdown toggle
let profile = document.querySelector('.profile');
let menu = document.querySelector('.menu');

profile.onclick = function () {
    menu.classList.toggle('active');
}

// Cart count AJAX
$(document).ready(function () {
    $.ajax({
        url: '/EndUser/Cart/GetCartCount',
        type: 'GET',
        success: function (data) {
            console.log("✅ Cart count received:", data);
            $('#lblCartCount').text(data);
        },
        error: function (xhr, status, error) {
            console.error("❌ AJAX error:", error);
            console.log(xhr.responseText);
        }
    });
});

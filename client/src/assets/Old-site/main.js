// Variables

var bigNavWidth = window.innerWidth;
var smallNavWidth = bigNavWidth * 80/100;
var allList = document.getElementsByClassName("all-list");

var slideIndex = 0;
var next = document.querySelector(".slide-container .next");
var previous = document.querySelector(".slide-container .previous");
slideSelect(slideIndex);


// All list width and center alignment with container div's
if(bigNavWidth >= 414) {
    for (var i = 0; i < allList.length; i++) {
        allList[i].style.cssText = "width:calc(" + (smallNavWidth * 20/100) + "px);\
        left:calc(-" + (smallNavWidth * 10/100) + "px + 50%);";
    }
}

if(bigNavWidth <= 461){
    for (var i = 0; i < allList.length; i++) {
        allList[i].style.cssText = "width:90px;\
                                    left:calc(-50%);";
    }
}
//Showing selected slide
function nextSlide(n) {
    slideSelect(slideIndex += n);
}
function slideSelect(selected) {
    var slides = document.getElementsByClassName("my-slides");
    var dots = document.getElementsByClassName("dot");
    
    slideIndex = selected;
    if(selected >= slides.length){
        slideIndex = 0;
    }
    if(selected < 0){
        slideIndex = slides.length - 1;
    }
    for(var i = 0; i < slides.length; i++){
        if(slides[i].style.display == "block"){

            slides[i].style.display = "none";
            dots[i].style.backgroundColor = "#222"
        }
    }
    slides[slideIndex].style.cssText = "display: block;";
    dots[slideIndex].style.cssText = "background-color: var(--main-color);";
}


/* Still On Progress */

console.log(bigNavWidth)
console.log(smallNavWidth)

// function show() {
//     if (!displayed){
//         Nav[1].querySelector(".all-list").style.cssText = "display: block;";
//         displayed = true;
//     }
//     else {
//         Nav[1].querySelector(".all-list").style.cssText = "display: none;";
//         displayed = false;
//     }
// }


// All button click
// var txt = "Hello JS , CSS , HTML";
// alert(txt.slice(txt.indexOf("JS"), txt.lastIndexOf("CSS")+3));
// function testCode(event) {
//     console.log(event.clientX);
// }
// var top_nav = document.querySelector(".products-nav");
// top_nav.addEventListener("click", testCode);
'use strict';

let country = document.getElementsByClassName('country');
let aboutCountryTours = document.getElementsByClassName('aboutCountryTours');

for (let i = 0; i < country.length; i++) {

	country[i].addEventListener('click', () => {
		let display = aboutCountryTours[i].style.display;
        if (aboutCountryTours[i].style.display == "none")
	    {
	      document.getElementsByClassName('aboutCountryTours')[i].style.display = "block";
	    } else {
	      document.getElementsByClassName('aboutCountryTours')[i].style.display = "none";
	    } 
	});
}

let delet = document.getElementsByClassName('deleteTour');
let idTour = document.getElementsByClassName('idTour');

for (let i = 0; i < delet.length; i++) {

    delet[i].addEventListener('click', () => {

        let delTour = confirm("Delete this tour?");
        if (delTour == true) {
            let id = parseInt(idTour[i].innerHTML);
            DeleteTour(id);
        }   
    });
}

function DeleteTour(id) {
    $.ajax({
        method: "GET",
        url: "/Home/Delet?id=" + id,
        data: id,
        success: function (context) {
            alert(context);
        },
        error: function (errorData) {
            alert("Error!" + errorData);
        },
    });

    location.replace("/Home/Index");
}

let addTour = document.getElementById('add');

addTour.addEventListener('click', () => {

    let nameCountry = $('#nameCountry').val();
        nameCountry = encodeURIComponent(nameCountry);
    let nameHotel = $('#nameHotel').val();
        nameHotel = encodeURIComponent(nameHotel);
    let dateArrival = $('#dateArrival').val();
    let price = parseInt($('#price').val());
    let aboutHotel = $('#aboutHotel').val();
        aboutHotel = encodeURIComponent(aboutHotel);
    let amountDay = parseInt($('#amountDay').val());

    if (nameCountry != "" && nameHotel != "" && price != 0 && aboutHotel != "" && amountDay != 0) {
        $.ajax({
            method: "POST",
            url: "/Home/Add",
            data: { nameCountry, nameHotel, dateArrival, price, aboutHotel, amountDay },
            success: function (context) {
                alert(context);
            },
            error: function (errorData) {
                alert("Error!" + errorData);
            },
        });

        location.replace("/Home/Index");
    } else {
        alert("Form`s fields are not filled!")
    }
 
});





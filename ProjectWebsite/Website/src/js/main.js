$(() => {
  console.log("====================================");
  console.log("The Dom is Ready");
  console.log("====================================");

  var state = true;

  $("#menu-btn").on("click", () => {
    $("#sidebar").toggleClass("active-nav");
    $(".my-container").toggleClass("active-cont");
    $("#menu-btn").css({
      transition: "0.4s",
      left: state ? "40px" : "190px",
    });
    state = !state;
  });

  var response;
  $.ajax({
    type: "GET",
    url: "html/Survey.html",
    async: false,
    success: function (text) {
      response = text;
    },
  });

});


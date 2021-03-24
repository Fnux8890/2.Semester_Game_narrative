$(() => {
  console.log("====================================");
  console.log("The Dom is Ready");
  console.log("====================================");

  var state = true;

  $("#menu-btn").on("click", () => {
    $("#sidebar").toggleClass("active-nav");
    $(".my-container").toggleClass("active-cont");
    $("#menu-btn").css({
      'transition': '0.4s',
      "left": state ? '5%' : '17%'
    })
    state = !state;
  });
});

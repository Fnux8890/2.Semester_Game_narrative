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

      var buildUrl = "../../Demo/Demo 0.1/Build";
      var loaderUrl = buildUrl + "/Unity Builds.loader.js";
      var config = {
        dataUrl: buildUrl + "/Unity Builds.data.gz",
        frameworkUrl: buildUrl + "/Unity Builds.framework.js.gz",
        codeUrl: buildUrl + "/Unity Builds.wasm.gz",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "DefaultCompany",
        productName: "The Narrative RPG game",
        productVersion: "0.1",
      };

      var container = document.querySelector("#unity-container");
      var canvas = document.querySelector("#unity-canvas");
      var loadingBar = document.querySelector("#unity-loading-bar");
      var progressBarFull = document.querySelector("#unity-progress-bar-full");
      var fullscreenButton = document.querySelector("#unity-fullscreen-button");
      var mobileWarning = document.querySelector("#unity-mobile-warning");

      if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
        container.className = "unity-mobile";
        config.devicePixelRatio = 1;
        mobileWarning.style.display = "block";
        setTimeout(() => {
          mobileWarning.style.display = "none";
        }, 5000);
      } else {
        canvas.style.width = "960px";
        canvas.style.height = "600px";
      }
      loadingBar.style.display = "block";

      var script = document.createElement("script");
      script.src = loaderUrl;
      script.onload = () => {
        createUnityInstance(canvas, config, (progress) => {
          progressBarFull.style.width = 100 * progress + "%";
        }).then((unityInstance) => {
          loadingBar.style.display = "none";
          fullscreenButton.onclick = () => {
            unityInstance.SetFullscreen(1);
          };
        }).catch((message) => {
          alert(message);
        });
      };
      document.body.appendChild(script);

});


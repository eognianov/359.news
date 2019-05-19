$(document).ready(function() {
  const apiKey = "837cfdfc79654129a9a0d4de5cb1d577";
  const apiUrl = "https://newsapi.org/v2/top-headlines?country=us&";
  let newsArr = [];
  let count = 1;
  let index;
  let lat;
  let long;
  let temp;
  let conditions;
  let date = new Date();
  let startDate = new Date("1981-05-24");
  let milisec = date.getTime() - startDate.getTime();
  let volNum = Math.round(milisec / (1000 * 60 * 60 * 24));
  let currDate = date.toDateString();

  getData();
  getLocation();

  function getData() {
    $.ajax({
      type: "GET",
      url: apiUrl + "apiKey=" + apiKey,
      datatype: "jsonp",
      success: function(data) {
        newsArr.push(data);
        insertData();
      }
    });
  }

  function insertData() {
    $(".date").append(currDate);
    $(".vol").append("Vol# " + volNum);
    for (let i = 1; i < newsArr[0].articles.length; i++) {
      index = i - 1;
      let news = newsArr[0].articles[index];
      if (news.description && news.source.id != "fox-news") {
        if (count < 4 && news.urlToImage) {
          $(".image" + count).attr("src", news.urlToImage);
          printData(news);
        } else if (count >= 4) {
          printData(news);
        }
        if (count === 8) {
          return true;
        }
      }
    }
  }

  function printData(news) {
    $(".storyTitle" + count).append(news.title);
    $(".story" + count).append(news.description);
    $(".by" + count).append("By: " + news.source.name);
    $(".a" + count).attr("href", news.url);
    count++;
  }

  function getLocation() {
    $.ajax({
      type: "GET",
      url: "https://extreme-ip-lookup.com/json/",
      success: function(data) {
        long = data.lon;
        lat = data.lat;
        getWeather();
      }
    });
  }

  function getWeather() {
    $.ajax({
      type: "GET",
      url:
        "https://api.darksky.net/forecast/3a616888226060540aaa82c98bfacdb9/" +
        lat +
        "," +
        long,
      dataType: "jsonp",
      success: function(data) {
        showWeather(data);
      }
    });
  }

  function showWeather(data) {
    conditions = data.currently.summary;
    temp = Math.round(data.currently.apparentTemperature);
    $(".conditions").append(conditions);
    $(".temp").append(temp + String.fromCharCode(176) + "F");
  }
});

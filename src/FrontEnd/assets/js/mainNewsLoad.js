$(() => {
    let moreNewsBtn = $('#mainNewsLoad');
    let mainNewsList = $('#mainNewsPosts > ul.posts');

    loadMainNews();

    moreNewsBtn.on('click', loadMainNews);

    function addNews(element) {
                    let newNews = $('<li><article><header><h3></h3><time></time></header><a class="image"><img src="/images/pic08.jpg"></a></article></li>');
                    newNews.find('h3').html(element.Title);
                    newNews.find('time').html(element.CreatedOn);
                    newNews.find('a.image').attr({ href: element.OriginalUrl, target: "_blank" });
                    newNews.find('img').attr('src', element.ImageUrl);
                    mainNewsList.append(newNews);
    }
			
    function loadMainNews() {
        let loadedNews = $('#mainNewsPosts > ul.posts li').length;
        moreNewsBtn.attr("disabled", true).addClass("btnLoading", {
            complete: mainNewsService.gatLastMainNews(5, loadedNews).then((mainNews) => {
                mainNews.map((element) => addNews(element), moreNewsBtn.removeAttr("disabled").removeClass('btnLoading'));
            }).catch(function (err) {
                console.log(err);
            })
        });
    }
});
$(() => {
    let moreNewsBtn = $('#mainNewsLoad');
    let mainNewsList = $('#mainNewsPosts > ul.posts');
    // mainNewsService.gatLastMainNews(5,loaded).then((mainNews) => {
    //     mainNews.forEach(element => {
    //         let newNews = $('<li><article><header><h3>ZAGLAVIE</h3><time>DATA</time></header><a class="image"><img src="/images/pic08.jpg"></a></article></li>');
    //         newNews.find('h3').val(element.title);
    //         newNews.find('time').val(element.CreatedOn);
    //         newNews.find('a.image').attr('href', element.OriginalUrl);
    //         newNews.find('img').attr('src', element.ImageUrl);
    //         mainNewsList.append(newNews);
    //         loaded += 5;
    //     });
    // }).catch(function (err) {
    //     console.log(err);
    // });

    loadMainNews();

    moreNewsBtn.on('click', loadMainNews);

    function Loading(btn) {
       return btn.attr("disabled").then(btn.addClass("btnLoading"));
    }

    function Loaded(btn) {
        return btn.removeAttr("disabled").then(btn.removeClass("btnLoading"));
    }

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
        moreNewsBtn.addClass("btnLoading", {
            complete: mainNewsService.gatLastMainNews(5, loadedNews).then((mainNews) => {
                mainNews.map((element) => addNews(element), moreNewsBtn.removeClass('btnLoading'));
            }).catch(function (err) {
                console.log(err);
            })
        });
    }

    return {
        moreNewsBtn,
        Loading,
        Loaded
    };
});
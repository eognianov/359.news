const mainNewsService = (() => {
	function gatLastMainNews(take, loaded) {
	  return kinvey.get('appdata', 'mainNews', 'kinvey', take, loaded);
	}

	function getAllMainNews() {
		return kinvey.get('appdata', 'mainNews', 'kinvey');
	}

	return {
		gatLastMainNews,
		getAllMainNews
	};
})();
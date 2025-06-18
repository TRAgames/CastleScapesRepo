mergeInto(LibraryManager.library, {

RateGame: function () {
  ysdk.feedback.canReview()
  .then(({ value, reason }) => {
    if (value) {
        ysdk.feedback.requestReview()
        .then(({ feedbackSent }) => {
            console.log(feedbackSent);
        })
    } else {
        console.log(reason)
    }
})
},

SetLeaderboardExtern: function (value) {
    ysdk.getLeaderboards()
        .then(lb => {
    lb.setLeaderboardScore('levels', value);
  });
},

SaveExtern: function (data) {
        try {

    var dataString = UTF8ToString(data);
    var myobj = JSON.parse(dataString);
    player.setData(myobj);

    } catch (e) {

myGameInstance.SendMessage('Progress','SaveEmpty');

        }
},

Log: function (data) {
    console.log(data);
},

LoadExtern: function () {
        try {
        if (!PlayerExist) {
            myGameInstance.SendMessage('Progress', 'LoadEmpty'); 
            getLanguage();
            //getTypeDevice();
       } else {
        player.getData().then(_data => {
            const myJSON = JSON.stringify(_data);
            myGameInstance.SendMessage('Progress', 'Load', myJSON); 
            getLanguage();
            //getTypeDevice();
            GameReady();  
            myGameInstance.SendMessage('Canvas', 'DeactivateBlackLoadingScreen'); 
            myGameInstance.SendMessage('Canvas', 'ActivateMenuUIScreen');
            //myGameInstance.SendMessage('Launcher', 'TurnOffAllSound', osInfo.name);  
            console.log(osInfo.name + detectBrowser());
        });
       }
       } catch (e) {
    myGameInstance.SendMessage('Progress','LoadEmpty');
    getLanguage();
    //getTypeDevice();
    GameReady(); 
}

},

SkipLevel: function () {
ysdk.adv.showRewardedVideo({
    callbacks: {
        onOpen: () => {
          //myGameInstance.SendMessage('Progress', 'PauseMusic');
        },
        onRewarded: () => {
          myGameInstance.SendMessage('Canvas', 'SkipLevelRewarded');
        },
        onClose: () => {
          //myGameInstance.SendMessage('Progress', 'UnpauseMusic');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
    }
})
},

GetCoins: function () {
ysdk.adv.showRewardedVideo({
    callbacks: {
        onOpen: () => {
          //myGameInstance.SendMessage('Progress', 'PauseMusic');
        },
        onRewarded: () => {
          myGameInstance.SendMessage('Canvas', 'GetCoinsRewarded');
        },
        onClose: () => {      
          //myGameInstance.SendMessage('Progress', 'UnpauseMusic');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
    }
})
},

DoubleCoins: function () {
ysdk.adv.showRewardedVideo({
    callbacks: {
        onOpen: () => {
          //myGameInstance.SendMessage('Progress', 'PauseMusic');
        },
        onRewarded: () => {
          myGameInstance.SendMessage('Canvas', 'DoubleCoinsRewarded');
        },
        onClose: () => {   
          //myGameInstance.SendMessage('Progress', 'UnpauseMusic');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
    }
})
},

FinishLevel: function () {
ysdk.adv.showFullscreenAdv({
    callbacks: {
        onOpen: function() {
          //myGameInstance.SendMessage('Progress', 'PauseMusic');
        },
        onClose: function(wasShown) {
          //myGameInstance.SendMessage('Progress', 'UnpauseMusic');
        },
        onError: function(error) {
          // some action on error
        }
    }
})
},

});
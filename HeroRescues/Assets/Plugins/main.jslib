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

SaveExtern: function (data) {
    var dataString = UTF8ToString(data);
    var myobj = JSON.parse(dataString);
    player.setData(myobj);
},

LoadExtern: function () {
    player.getData().then(_data => {
        const myJSON = JSON.stringify(_data);
        myGameInstance.SendMessage('Progress','Load',myJSON);
    });
},

GetHint: function () {
ysdk.adv.showRewardedVideo({
    callbacks: {
        onOpen: () => {
          myGameInstance.SendMessage('Progress', 'PauseMusic');
        },
        onRewarded: () => {
          console.log('Rewarded!');
        },
        onClose: () => {
          myGameInstance.SendMessage('Main Camera', 'ShowHintRewarded');
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
          myGameInstance.SendMessage('Progress', 'PauseMusic');
        },
        onClose: function(wasShown) {
          myGameInstance.SendMessage('Progress', 'UnpauseMusic');
        },
        onError: function(error) {
          // some action on error
        }
    }
})
},

});
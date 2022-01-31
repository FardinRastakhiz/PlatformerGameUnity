var LoLWebGL = {
  _GameIsReady: function(
    gameName,
    callbackObject,
    aspectRatio,
    resolution,
    sdkVersion
  ) {
    const targetGameObject = Pointer_stringify(callbackObject);
    console.log('GameIsReady() from JS');
    console.log('Game Name: ' + Pointer_stringify(gameName));
    console.log('LoL UNITY SDK version: ' + Pointer_stringify(sdkVersion));
    console.log('Sending data to GameObject' + targetGameObject);

    const EVENT = {
      UNITY: {
        PAUSE: 'PauseMessageReceived',
        RESUME: 'ResumeMessageReceived',
        QUESTION_LIST: 'QuestionListReceived',
        LANGUAGE_DEFS: 'LanguageListReceived',
        START_GAME: 'StartGameDataReceived',
        PLAYER_ACTIVITY_ID: 'PlayerActivityIdReceived',
        LOAD_STATE: 'StateDataReceived',
        SAVE_STATE_RESULT: 'SaveResultDataReceived',
        ANSWER_RESULT: 'AnswerResultDataReceived'
      },
      RECEIVED: {
        PAUSE: 'pause',
        RESUME: 'resume',
        QUESTIONS: 'questions',
        LANGUAGE: 'language',
        START: 'start',
        INIT: 'init',
        PLAYER_ACTIVITY_ID: 'playerActivityId',
        LOAD_STATE: 'loadState',
        SAVE_STATE_RESULT: 'saveStateResult',
		    ANSWER_RESULT: "answerResult"
      }
    };

    // const LANGUAGE_PAYLOAD = {
    // 	_meta: {
    // 		maxChars: {
    // 			welcome: 20
    // 		}
    // 	},
    // 	en: {
    // 		welcome: 'welcome'
    // 	},
    // 	es: {
    // 		welcome: "¡Bienvenido!"
    // 	}
    // };

    const START_GAME_PAYLOAD = {
      languageKey: 'en',
      languageName: 'English',
      lastProgress: JSON.stringify({
        gameProvidedPayload: {
          level: 5,
          stars: 5000,
        },
      }),
    };

    window.addEventListener('message', function(msg) {
      console.log('[PARENT => UNITY]', msg);

      switch (msg.data.messageName) {
        case EVENT.RECEIVED.PAUSE:
          SendMessage(targetGameObject, EVENT.UNITY.PAUSE);
          break;
        case EVENT.RECEIVED.RESUME:
          SendMessage(targetGameObject, EVENT.UNITY.RESUME);
          break;
        case EVENT.RECEIVED.QUESTIONS:
          SendMessage(
            targetGameObject,
            EVENT.UNITY.QUESTION_LIST,
            msg.data.payload
          );
          break;
        case EVENT.RECEIVED.LANGUAGE:
          SendMessage(
            targetGameObject,
            EVENT.UNITY.LANGUAGE_DEFS,
            msg.data.payload
          );
          break;
        case EVENT.RECEIVED.START:
          SendMessage(
            targetGameObject,
            EVENT.UNITY.START_GAME,
            msg.data.payload
          );
          break;
        case EVENT.RECEIVED.ANSWER_RESULT:
          SendMessage(
            targetGameObject,
            EVENT.UNITY.ANSWER_RESULT,
            msg.data.payload
          );
          break;
        case EVENT.RECEIVED.PLAYER_ACTIVITY_ID:
	        SendMessage(
            targetGameObject,
            EVENT.UNITY.PLAYER_ACTIVITY_ID,
            msg.data.payload
          );
	        break;
        case EVENT.RECEIVED.LOAD_STATE:
	        SendMessage(
            targetGameObject,
            EVENT.UNITY.LOAD_STATE,
            msg.data.payload || ""
          );
	        break;
        case EVENT.RECEIVED.SAVE_STATE_RESULT:
	        SendMessage(
            targetGameObject,
            EVENT.UNITY.SAVE_STATE_RESULT,
            msg.data.payload
          );
	        break;
        case 'init':
          break;
        default:
          console.log('Unhandled message: ' + msg);
      }
    });

    // Calls init on parent (gameframe)
    parent.postMessage(
      {
        message: 'init',
        payload: JSON.stringify({
          aspectRatio: aspectRatio,
          resolution: resolution,
          sdkVersion: Pointer_stringify(sdkVersion),
        }),
      },
      '*'
    );

    setTimeout(function() {
      // Fake Language. Todo: send 'language' event from harness
      // SendMessage(targetGameObject, EVENT.UNITY.LANGUAGE_DEFS,
      // 	JSON.stringify(LANGUAGE_PAYLOAD)
      // );
      // Fake Start: todo: send 'start' event from harness
      // SendMessage(targetGameObject, EVENT.UNITY.START_GAME,
      // 	JSON.stringify(START_GAME_PAYLOAD)
      // );
    }, 1);
  },

  _PostWindowMessage: function(_messageName, _jsonPayload) {
    const messageName = Pointer_stringify(_messageName);
    const jsonPayload = Pointer_stringify(_jsonPayload);
    const payload = {
      message: messageName,
      payload: jsonPayload,
    };
    parent.postMessage(payload, '*');
  },
};

// autoAddDeps(LibraryWebSockets, '$webSocketInstances');
mergeInto(LibraryManager.library, LoLWebGL);

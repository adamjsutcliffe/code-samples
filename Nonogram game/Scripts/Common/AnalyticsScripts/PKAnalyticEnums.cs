//
//  PKAnalyticEnums.h
//  Sharper
//
//  Copyright (c) 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
  
  public enum SHRDeviceAudioPortType 
  {
    
      SHRDeviceAudioPortTypeLineOut = 1,
    
      SHRDeviceAudioPortTypeHeadphones = 2,
    
      SHRDeviceAudioPortTypeBluetooth = 3,
    
      SHRDeviceAudioPortTypeSpeaker = 4,
    
      SHRDeviceAudioPortTypeAirplay = 5,
    
      SHRDeviceAudioPortTypeOther = 6,
    
  }
  
  public enum QLGameSourceType 
  {
    
      QLGameSourceTypeStart = 0,
    
      QLGameSourceTypeQuit = 1,
    
      QLGameSourceTypeFinish = 2,
    
      QLGameSourceTypePause = 3,
    
      QLGameSourceTypeResume = 4,
    
      QLGameSourceTypeRestart = 5,
    
  }
  
  public enum QLGameType 
  {
    
      QLGameTypeNormal = 0,
    
      QLGameTypeReplayNormal = 1,
    
      QLGameTypeGold = 2,
    
      QLGameTypeReplayGold = 3,
    
  }
  
  public enum QLAdResultType 
  {
    
      QLAdResultTypeWatched = 0,
    
      QLAdResultTypeAborted = 1,
    
      QLAdResultTypeError = 2,
    
      QLAdResultTypeRequested = 3,
    
      QLAdResultTypeStarted = 4,
    
  }
  
  public enum QLAdType 
  {
    
      QLAdTypeNotSet = 0,
    
      QLAdTypeRewarded = 1,
    
      QLAdTypeInterstitial = 2,
    
      QLAdTypeBanner = 3,
    
  }
  
  public enum QLAdSourceType 
  {
    
      QLAdSourceTypeNotSet = 0,
    
      QLAdSourceTypeInGame = 1,
    
      QLAdSourceTypeReplayPopUp = 2,
    
      QLAdSourceTypeGoldCardPopUp = 3,
    
      QLAdSourceTypeFilmPopUp = 4,
    
      QLAdSourceTypePostGame = 5,

      QLAdSourceTypeAndroidGold = 6

    }
  
  public enum QLHintType 
  {
    
      QLHintTypeFreeHint = 0,
    
      QLHintTypePaidHint = 1,
    
  }
  
  public enum QLCoinSourceType 
  {
    
      QLCoinSourceTypePuzzleSolved = 0,
    
      QLCoinSourceTypeNewLocationUnlocked = 1,
    
      QLCoinSourceTypeVideoWatched = 2,
    
      QLCoinSourceTypeGoldCollectionComplete = 3,
    
      QLCoinSourceTypeStore = 4,
    
      QLCoinSourceTypePostGameVideo = 5,
    
  }
  
  public enum QLPurchaseState 
  {
    
      QLPurchaseStateStarted = 0,
    
      QLPurchaseStateSucceded = 1,
    
      QLPurchaseStateFailed = 2,
    
  }
  
  public enum QLPurchaseSource 
  {
    
      QLPurchaseSourceMenu = 0,
    
      QLPurchaseSourceCollection = 1,
    
      QLPurchaseSourceInGame = 2,
    
      QLPurchaseSourceHint = 3,
    
      QLPurchaseSourceReplay = 4,
    
      QLPurchaseSourceGold = 5,
    
      QLPurchaseSourceFilm = 6,
    
  }
  
  public enum QLABTestState 
  {
    
      QLABTestStateAssignment = 0,
    
      QLABTestStateExecuted = 1,
    
  }
  
  public enum QLFilmSourceType 
  {
    
      QLFilmSourceTypeRewardedVideo = 0,
    
      QLFilmSourceTypeCoinPurchase = 1,
    
      QLFilmSourceTypeDailyReward = 2,
    
  }
  
  public enum QLStoreOpenSource 
  {
    
      QLStoreOpenSourceMenu = 0,
    
      QLStoreOpenSourceCollection = 1,
    
      QLStoreOpenSourceInGame = 2,
    
      QLStoreOpenSourceHint = 3,
    
      QLStoreOpenSourceReplay = 4,
    
      QLStoreOpenSourceGold = 5,
    
      QLStoreOpenSourceFilm = 6,
    
  }
  }

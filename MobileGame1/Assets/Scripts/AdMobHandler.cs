// this code is party of the unity project targeting android and is to be used with unity-plugin-library.jar
// and should be attached to the main camera

using UnityEngine;
using System.Collections;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdMobHandler : MonoBehaviour {
	public BannerView bannerView;
	public InterstitialAd interstitial;

	void Start () {
	
		// Keep this object around for entire game
		DontDestroyOnLoad(transform.gameObject);
		
		AdRequest request = new AdRequest.Builder().Build();
		
		// Banner view ad always present at bottom of screen
		//bannerView = new BannerView("ca-app-pub-6877754457671498/9780611568", AdSize.Banner, AdPosition.Bottom);
		//bannerView.LoadAd(request);
		
		// Interstitial ad only present before play begins
		interstitial = new InterstitialAd("ca-app-pub-6877754457671498/4235971961");

		// Register for ad events.
		interstitial.AdLoaded += delegate(object sender, System.EventArgs args) {};
		interstitial.AdFailedToLoad += delegate(object sender, AdFailedToLoadEventArgs args) {};
		interstitial.AdOpened += delegate(object sender, System.EventArgs args) {};
		interstitial.AdClosing += delegate(object sender, System.EventArgs args) {};
		interstitial.AdClosed += delegate(object sender, System.EventArgs args) {};
		interstitial.AdLeftApplication += delegate(object sender, System.EventArgs args) {};
		
		interstitial.LoadAd(request);		
		
	}
	
	public void ShowInterstitialAd()
	{
		if (interstitial.IsLoaded ())
			interstitial.Show();		
	}
	
	void Update () {
		//hideadmob();		
	}
	
	
}
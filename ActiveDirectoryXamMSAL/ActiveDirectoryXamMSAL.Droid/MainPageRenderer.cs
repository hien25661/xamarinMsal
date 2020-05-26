
using Android.App;
using Android.Content;
using Xamarin.Forms.Platform.Android;
using ActiveDirectoryXamMSAL;
using Xamarin.Forms;
using ActiveDirectoryXamMSAL.Droid;

[assembly: ExportRenderer(typeof(MainPage), typeof(MainPageRenderer))]
namespace ActiveDirectoryXamMSAL.Droid
{
    class MainPageRenderer : PageRenderer
    {
        public MainPageRenderer(Context context) : base(context)
        {

        }
        MainPage page;

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            page = e.NewElement as MainPage;
            var activity = this.Context as Activity;           
        }

    }
}
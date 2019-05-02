using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Grayeed
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        public GrayedInfo Info { get; set; }

        public MapPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }

        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /// 

      




        async void DrawRouteBetweenMeAndSpecificPoint(double _lat, double _long)
        {

            BasicGeoposition startLocation = new BasicGeoposition();

            
            Geolocator geo = null;
            if (geo == null)
            {
                geo = new Geolocator();
            }
            Geoposition pos = await geo.GetGeopositionAsync();





            startLocation.Latitude = pos.Coordinate.Point.Position.Latitude;
            startLocation.Longitude = pos.Coordinate.Point.Position.Longitude;

       


            Geopoint startPoint = new Geopoint(startLocation);

            BasicGeoposition endLocation = new BasicGeoposition();
            endLocation.Latitude = _lat;
            endLocation.Longitude = _long;
            Geopoint endPoint = new Geopoint(endLocation);
            // Get a route as shown previously.
            MapRouteFinderResult routeResult =
               await MapRouteFinder.GetDrivingRouteAsync(
               startPoint,
               endPoint,
               MapRouteOptimization.Time,
               MapRouteRestrictions.None);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                MyMap.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                await MyMap.TrySetViewBoundsAsync(
                    routeResult.Route.BoundingBox,
                    null,
                    Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            
            Info = e.Parameter as GrayedInfo;


            Geolocator geolocator = new Geolocator();
            Geoposition geoposition = null;
            try
            {
                geoposition = await geolocator.GetGeopositionAsync();
            }
            catch (Exception ex)
            {
                // Handle errors like unauthorized access to location services or no Internet access.
            }
            MapIcon mapIcon = new MapIcon();
            mapIcon.Image = RandomAccessStreamReference.CreateFromUri(
              new Uri("ms-appx:///Assets/IC488534.png"));
            mapIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);

             

            
           double latitude = Convert.ToDouble(Info.Lat);
            double longtude = Convert.ToDouble(Info.Long);
            mapIcon.Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = longtude,
                Longitude = latitude
            });
      //mapIcon.Title = Info.Name;
            MyMap.MapElements.Add(mapIcon);
            MyMap.Center = mapIcon.Location;
            MyMap.ZoomLevel = 15;
            

          



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            double latitude = Convert.ToDouble(Info.Long);
            double longtude = Convert.ToDouble(Info.Lat);
            DrawRouteBetweenMeAndSpecificPoint(latitude, longtude);
            
          

        }

       
    }
}

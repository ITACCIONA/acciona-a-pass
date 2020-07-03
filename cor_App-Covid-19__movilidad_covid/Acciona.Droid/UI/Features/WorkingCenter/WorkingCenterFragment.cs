using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Droid.UI;
using Acciona.Domain.Model;
using System.Threading.Tasks;
using Droid.Utils;
using Android;
using Android.Support.V4.Content;
using Android.Net;
using Acciona.Presentation.UI.Features.WorkingCenter;
using Acciona.Domain.Model.Employee;
using Acciona.Domain.Model.Security;
using Acciona.Droid.UI.Controls;

namespace Acciona.Droid.UI.Features.WorkingCenter
{
    public class WorkingCenterFragment : BaseFragment<WorkingCenterPresenter>, WorkingCenterUI
    {
        private View buttonBack;        
        private View buttonModify;
        private string other;
        private ListStringEditText listPais;
        private ListStringEditText listCiudad;
        private ListEditText listCentro;
        private Location selected;
        private IEnumerable<Location> locations;
        private List<string> paises;
        private List<string> ciudades;
        private Ficha ficha;

        internal static WorkingCenterFragment NewInstance(Ficha ficha)
        {
            var fragment = new WorkingCenterFragment();
            fragment.SetFicha(ficha);
            return fragment;
        }

        private void SetFicha(Ficha ficha)
        {
            this.ficha = ficha;
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.SetFicha(ficha);
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.working_center_fragment;
        }

        protected override void AssingViews(View view)
        {
            buttonBack = view.FindViewById(Resource.Id.buttonBack);
            buttonBack.Click += (o, e) => presenter.BackClicked();

            buttonModify = view.FindViewById(Resource.Id.buttonModify);
            buttonModify.Click += ButtonModify_Click;
            other = GetString(Resource.String.center_other);

            
            listPais = view.FindViewById<ListStringEditText>(Resource.Id.listPais);
            listPais.ItemChanged += (o, pais) =>
            {
                if (pais.Equals(other))
                {
                    listCiudad.SetListableObjects(new List<string>() { other });
                    listCentro.SetListableObjects(new List<Location>() { new Location() { Name = other, Pais = other, Ciudad = other, IdLocation = -1 } });
                }
                else
                {
                    var filtered = locations.Where(x => x.Pais.Equals(pais)).OrderBy(x => x.Name).ToList();
                    filtered.Insert(0, new Location() { Name = other, Pais=pais, Ciudad = other, IdLocation = -1 });
                    listCentro.SetListableObjects(filtered);
                    ciudades = locations.Where(x => x.Pais.Equals(pais)).Select(x=>x.Ciudad).Distinct().ToList();
                    ciudades.Sort();
                    ciudades.Insert(0, other);
                    listCiudad.SetListableObjects(ciudades);                    
                }                
            };
            listCiudad = view.FindViewById<ListStringEditText>(Resource.Id.listCiudad);
            listCiudad.ItemChanged += (o, ciudad) =>
            {
                if (ciudad.Equals(other))
                {
                    var filtered = locations.Where(x => x.Pais.Equals(listPais.Text)).OrderBy(x => x.Name).ToList();
                    filtered.Insert(0, new Location() { Name = other, Pais = listPais.Text, Ciudad = other, IdLocation = -1 });
                    listCentro.SetListableObjects(filtered);
                }
                else
                {
                    var filtered = locations.Where(x => x.Pais.Equals(listPais.Text) && x.Ciudad.Equals(ciudad)).OrderBy(x => x.Name).ToList();
                    filtered.Insert(0, new Location() { Name = other, Pais = listPais.Text, Ciudad = ciudad, IdLocation = -1 });
                    listCentro.SetListableObjects(filtered);
                }                
            };
            listCentro = view.FindViewById<ListEditText>(Resource.Id.listCentro);
            listCentro.ItemChanged += (o, loc ) =>
            {
                var l = loc as Location;                                
                int index = ciudades.IndexOf(l.Ciudad);                
                listCiudad.SetSelectionIndex(index);
                if (index >= 0 && l.IdLocation == -1)
                    l.Ciudad = ciudades[index];
                
            };
        }

        private void ButtonModify_Click(object sender, EventArgs e)
        {
            var loc = listCentro.GetSelection() as Location;            
            presenter.ModifyClicked(loc);
        }

        public void SetSelectedLocation(Location location)
        {
            this.selected = location;
            if (selected != null)
            {
                listPais.Text = location?.Pais;
                listCiudad.Text = location?.Ciudad;
                listCentro.Text = location?.Name;
            }
            else
            {
                //buttonBack.Visibility = ViewStates.Invisible;
            }
        }

        public void SetLocations(IEnumerable<Location> locations)
        {
            this.locations = locations;
            paises = locations.Select(x => x.Pais).Distinct().ToList();
            paises.Sort();
            paises.Insert(0,other);
            listPais.SetListableObjects(paises);            
            if (selected == null)
                listPais.ForceInvokeEvent();
            else
            {                                
                int indexPais = paises.IndexOf(selected.Pais);
                listPais.SetSelectionIndex(indexPais);
                ciudades = locations.Where(x => x.Pais.Equals(selected.Pais)).Select(x => x.Ciudad).Distinct().ToList();
                ciudades.Sort();
                ciudades.Insert(0, other);
                listCiudad.SetListableObjects(ciudades);
                var index = ciudades.IndexOf(selected.Ciudad);
                listCiudad.SetSelectionIndex(index);
                var filtered = locations.Where(x => x.Pais.Equals(selected.Pais)&&x.Ciudad.Equals(selected.Ciudad)).OrderBy(x=>x.Name).ToList();                
                filtered.Insert(0, new Location() { Name = other, Pais = paises[indexPais], Ciudad = ciudades[index], IdLocation = -1 });
                var s=filtered.Where(x => x.IdLocation == selected.IdLocation).First();                                
                index =filtered.IndexOf(s);
                listCentro.SetListableObjects(filtered);
                listCentro.SetSelectionIndex(index);                
            }
        }
    }
}
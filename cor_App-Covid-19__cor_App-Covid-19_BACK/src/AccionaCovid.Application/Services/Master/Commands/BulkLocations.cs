using AccionaCovid.Application.Core;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// Servicio de volcado de localizaciones
    /// </summary>
    public class BulkLocations
    {
        /// <summary>
        /// Petición del volcado de localizaciones
        /// </summary>
        public class BulkLocationsRequest : IRequest<bool>
        {
            /// <summary>
            /// Fichero CSV con localizaciones
            /// </summary>
            public IFormFile LocationsFile { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class BulkLocationsCommandHandler : BaseCommandHandler<BulkLocationsRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Localizacion> repository;
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Area> areaRepository;
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Region> regionRepository;
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Pais> paisRepository;

            /// <summary>
            /// Constructor
            /// </summary>
            public BulkLocationsCommandHandler(IRepository<Localizacion> repository, IRepository<Area> areaRepository, IRepository<Region> regionRepository, IRepository<Pais> paisRepository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
                this.areaRepository = areaRepository ?? throw new ArgumentNullException(nameof(areaRepository));
                this.regionRepository = regionRepository ?? throw new ArgumentNullException(nameof(regionRepository));
                this.paisRepository = paisRepository ?? throw new ArgumentNullException(nameof(paisRepository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(BulkLocationsRequest request, CancellationToken cancellationToken)
            {
                (string[], string[][]) csv = await BulkUtils.ReadCsvAsync(request.LocationsFile, cancellationToken).ConfigureAwait(false);


                string[] headers = csv.Item1;
                string[][] data = csv.Item2;
                if (!data.Any())
                    return false;

                Localizacion.SetPropertyIndexes(headers);
                Area.SetPropertyIndexes(headers);
                Region.SetPropertyIndexes(headers);
                Pais.SetPropertyIndexes(headers);

                List<string> existentLocations = await repository.GetAll().Select(u => u.Nombre).ToListAsync().ConfigureAwait(false);
                List<Area> existentAreas = (await areaRepository.GetAll()
                    .Select(a => new {
                        a.Region.IdPais,
                        Area = a
                    }).ToListAsync().ConfigureAwait(false))
                    .Select(x => {
                        var aNew = x.Area;
                        aNew.IdPais = x.IdPais;
                        return aNew;
                    }).ToList();
                List<Region> existentRegions = await regionRepository.GetAll().ToListAsync().ConfigureAwait(false);
                List<Pais> existentPaises = await paisRepository.GetAll().ToListAsync().ConfigureAwait(false);

                List<Localizacion> newLocalizaciones = new List<Localizacion>();
                List<Area> newAreas = new List<Area>();
                List<Region> newRegions = new List<Region>();
                List<Pais> newPaises = new List<Pais>();

                int indexArea = existentAreas.Max(p => p.Id);
                int indexRegion = existentRegions.Max(p => p.Id);
                int indexPais = existentPaises.Max(p => p.Id);

                foreach (string[] dataRow in data)
                {
                    Localizacion newLocalizacion = new Localizacion(dataRow);
                    Area newArea = new Area(dataRow);
                    Region newRegion = new Region(dataRow);
                    Pais newPais = new Pais(dataRow);

                    if(!string.IsNullOrEmpty(newPais.Nombre))
                    {
                        if (existentPaises.Any(p => p.Nombre == newPais.Nombre))
                            newPais = existentPaises.Single(p => p.Nombre == newPais.Nombre);
                        else if(newPaises.Any(p => p.Nombre == newPais.Nombre))
                            newPais = newPaises.Single(p => p.Nombre == newPais.Nombre);
                        else
                        {
                            indexPais++;
                            newPais.Id = indexPais;
                            newPais.LastAction = "CREATE";
                            newPais.LastActionDate = DateTime.UtcNow;
                            newPaises.Add(newPais);
                        }

                        if (!string.IsNullOrEmpty(newRegion.Nombre))
                        {
                            if (existentRegions.Any(p => p.Nombre == newRegion.Nombre))
                                newRegion = existentRegions.Single(p => p.Nombre == newRegion.Nombre);
                            else if (newRegions.Any(p => p.Nombre == newRegion.Nombre))
                                newRegion = newRegions.Single(p => p.Nombre == newRegion.Nombre);
                            else
                            {
                                indexRegion++;
                                newRegion.Id = indexRegion;
                                newRegion.IdPais = newPais.Id;
                                newRegion.LastAction = "CREATE";
                                newRegion.LastActionDate = DateTime.UtcNow;
                                newRegions.Add(newRegion);
                            }
                        }
                        else if(!string.IsNullOrEmpty(newArea.Nombre))
                        {
                            if (existentRegions.Any(p => p.Nombre == newArea.Nombre))
                                newRegion = existentRegions.Single(p => p.Nombre == newArea.Nombre);
                            else if (newRegions.Any(p => p.Nombre == newArea.Nombre))
                                newRegion = newRegions.Single(p => p.Nombre == newArea.Nombre);
                            else
                            {
                                indexRegion++;
                                newRegion.Id = indexRegion;
                                newRegion.Nombre = newArea.Nombre;
                                newRegion.IdPais = newPais.Id;
                                newRegion.LastAction = "CREATE";
                                newRegion.LastActionDate = DateTime.UtcNow;
                                newRegions.Add(newRegion);
                            }
                        }

                        if (!string.IsNullOrEmpty(newArea.Nombre))
                        {
                            if (existentAreas.Any(p => p.Nombre == newArea.Nombre && p.IdPais == newPais.Id))
                                newArea = existentAreas.Single(p => p.Nombre == newArea.Nombre && p.IdPais == newPais.Id);
                            else if (newAreas.Any(p => p.Nombre == newArea.Nombre && p.IdPais == newPais.Id))
                                newArea = newAreas.Single(p => p.Nombre == newArea.Nombre && p.IdPais == newPais.Id);
                            else
                            {
                                indexArea++;
                                newArea.Id = indexArea;
                                newArea.IdRegion = newRegion.Id;
                                newArea.IdPais = newPais.Id;
                                newArea.LastAction = "CREATE";
                                newArea.LastActionDate = DateTime.UtcNow;
                                newAreas.Add(newArea);
                            }
                        }
                    }

                    if (existentLocations.Contains(newLocalizacion.Nombre))
                        continue;

                    newLocalizacion.IdArea = null;
                    if (!string.IsNullOrEmpty(newArea.Nombre))
                        newLocalizacion.IdArea = newArea.Id;

                    newLocalizacion.LastAction = "CREATE";
                    newLocalizacion.LastActionDate = DateTime.UtcNow;

                    newLocalizaciones.Add(newLocalizacion);
                }

                await paisRepository.BulkInsertAsync(newPaises).ConfigureAwait(false);
                await regionRepository.BulkInsertAsync(newRegions).ConfigureAwait(false);
                await areaRepository.BulkInsertAsync(newAreas).ConfigureAwait(false);

                await repository.BulkInsertAsync(newLocalizaciones).ConfigureAwait(false);

                //repository.AddRange(newLocalizaciones);
                //await repository.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
        }
    }
}

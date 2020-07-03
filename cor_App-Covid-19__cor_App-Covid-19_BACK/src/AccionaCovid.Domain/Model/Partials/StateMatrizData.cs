using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccionaCovid.Domain.Model
{
    /// <summary>
    /// Clase que representa la matriz de estado de un pasaporte
    /// </summary>
    public class StateMatrizData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCRs"></param>
        /// <param name="lastAnalisticIgG"></param>
        /// <param name="lastAnalisticIgM"></param>
        /// <param name="lastQuickTest"></param>
        /// <param name="lastFeber"></param>
        /// <param name="lastDeclFiebre"></param>
        /// <param name="lastDeclOtros"></param>
        /// <param name="lastDeclContato"></param>
        public StateMatrizData(List<ResultadoTestPcr> pCRs, ValoracionParametroMedico lastAnalisticIgG, ValoracionParametroMedico lastAnalisticIgM,
            ResultadoTestMedico lastQuickTest, ValoracionParametroMedico lastFeber, ResultadoEncuestaSintomas lastDeclFiebre, ResultadoEncuestaSintomas lastDeclOtros, ResultadoEncuestaSintomas lastDeclContato)
        {
            PCRs = pCRs;
            LastAnalisticIgG = lastAnalisticIgG;
            LastAnalisticIgM = lastAnalisticIgM;
            LastQuickTest = lastQuickTest;
            LastFeber = lastFeber;
            LastDeclFiebre = lastDeclFiebre;
            LastDeclOtros = lastDeclOtros;
            LastDeclContato = lastDeclContato;
        }

        public enum TypeState
        {
            Afectado = 1,
            Recaido = 2,
            Sintomas = 3,
            Contacto = 4,
            Traceo = 5,
            NoSintomas = 6,
            
        }

        #region Datos Entrada

        public List<ResultadoTestPcr> PCRs { get; private set; }

        public ValoracionParametroMedico LastAnalisticIgG { get; private set; }

        public ValoracionParametroMedico LastAnalisticIgM { get; private set; }

        public ResultadoTestMedico LastQuickTest { get; private set; }

        public ValoracionParametroMedico LastFeber { get; private set; }

        public ResultadoEncuestaSintomas LastDeclFiebre { get; private set; }

        public ResultadoEncuestaSintomas LastDeclOtros { get; private set; }

        public ResultadoEncuestaSintomas LastDeclContato { get; private set; }

        #endregion

        #region Datos Salida

        public bool PCRReconvertido 
        {
            get
            {
                if(PCRs == null || !PCRs.Any())
                {
                    return false;
                }

                List<ResultadoTestPcr> pCRs = PCRs.OrderBy(c => c.FechaTest).ToList();

                ResultadoTestPcr pcrNoPositivo = pCRs.LastOrDefault(c => !c.Positivo);
                ResultadoTestPcr pcrPositivo = pCRs.FirstOrDefault(c => c.Positivo);

                return pcrNoPositivo != null && pcrPositivo != null && pcrNoPositivo.FechaTest > pcrPositivo.FechaTest;
            }
        }

        public bool PCRUltimo
        {
            get
            {
                return PCRs == null || !PCRs.Any() ? false: PCRs.OrderBy(c => c.FechaTest).ToList().LastOrDefault().Positivo;
            }
        }

        public bool? TestInmuneIgG
        {
            get
            {
                if (LastAnalisticIgG != null || LastQuickTest != null)
                {
                    bool isInmune = LastAnalisticIgG != null ? LastAnalisticIgG.Valor : false;

                    if (!isInmune && LastQuickTest != null)
                    {
                        isInmune = LastQuickTest != null ? LastQuickTest.Igg : false;
                    }

                    return isInmune;
                }

                return null;
            }
        }

        public bool? TestInmuneIgM
        {
            get
            {
                if (LastAnalisticIgM != null || LastQuickTest != null)
                {
                    bool isInmune = LastAnalisticIgM != null ? LastAnalisticIgM.Valor : false;

                    if (!isInmune && LastQuickTest != null)
                    {
                        isInmune = LastQuickTest != null ? LastQuickTest.Igm : false;
                    }

                    return isInmune;
                }

                return null;
            }
        }

        public TypeState StateLevel 
        { 
            get
            {
                return CalculateStateLevel();
            }
        }

        private TypeState CalculateStateLevel()
        {
            if(PCRUltimo)
            {
                return PCRReconvertido ? TypeState.Recaido : TypeState.Afectado;
            }
            else
            {
                if(LastFeber?.Valor == true || LastDeclFiebre?.Valor == true || LastDeclOtros?.Valor == true)
                {
                    return TypeState.Sintomas;
                }

                if(LastDeclContato?.Valor == true)
                {
                    return TypeState.Contacto;
                }

                return TypeState.NoSintomas;
            }
        }

        public EstadoPasaporte NewState { get; private set; }

        #endregion

        public EstadoPasaporte Calculate(Dictionary<string, EstadoPasaporte> matrix)
        {
            // CASOS DE TRANSICION
            if(StateLevel == TypeState.Afectado)
            {
                return NewState = matrix["Afectado"];
            }

            if (StateLevel == TypeState.Recaido)
            {
                return NewState = TestInmuneIgG.HasValue && TestInmuneIgG.Value ? matrix["Recaida1"] : matrix["Recaida2"];
            }

            if(PCRReconvertido && !PCRUltimo && TestInmuneIgM == true)
            {
                return NewState = TestInmuneIgG.HasValue && TestInmuneIgG.Value ? matrix["Recaida1"] : matrix["Recaida2"];
            }

            // CASOS CALCULADOS
            List<EstadoPasaporte> partialMatriz = matrix.Values.Where(c => c.IdTipoEstadoNavigation.Nombre == StateLevel.ToString()).ToList();

            // Caso Azul
            if(!TestInmuneIgM.HasValue || !TestInmuneIgG.HasValue)
            {
                return NewState = partialMatriz.FirstOrDefault(c => c.TestInmuneIgG == null && c.TestInmuneIgM == null && (!PCRReconvertido ? c.Pcrant == null : c.Pcrant == PCRReconvertido));
            }

            if (TestInmuneIgM.HasValue && TestInmuneIgG.HasValue)
            {
                //Caso Verde
                if(PCRReconvertido)
                {
                    return NewState = partialMatriz.FirstOrDefault(c => c.TestInmuneIgG == TestInmuneIgG.Value && c.TestInmuneIgM == TestInmuneIgM.Value && c.Pcrant == PCRReconvertido);
                }

                //Caso Naranja
                if (!PCRReconvertido)
                {
                    return NewState = partialMatriz.FirstOrDefault(c => c.TestInmuneIgG == TestInmuneIgG.Value && c.TestInmuneIgM == TestInmuneIgM.Value && c.Pcrant == null);
                }
            }

            return null;
        }
    }
}

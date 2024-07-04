using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;

        private IEmpresaMonedaRepository _empresamonedas;

        private IAlmacenRepository _almacenes;

        private ISeccionRepository _secciones;

        private IAlmacenSeccionRepository _almacensecciones;

        private IUnidadMedidaRepository _unidadesmedida;

        private IConversionRepository _conversiones;

        private IArticuloRepository _articulos;

        private ISolicitudRepository _solicitudes;


        public IEmpresaMonedaRepository EmpresaMonedas
        {
            get
            {
                if (_empresamonedas == null)
                {
                    _empresamonedas = new EmpresaMonedaRepository(_repoContext);
                }
                return _empresamonedas;
            }
        }

        public IAlmacenRepository Almacenes
        {
            get
            {
                if (_almacenes == null)
                {
                    _almacenes = new AlmacenRepository(_repoContext);
                }
                return _almacenes;
            }
        }

        public ISeccionRepository Secciones
        {
            get
            {
                if (_secciones == null)
                {
                    _secciones = new SeccionRepository(_repoContext);
                }
                return _secciones;
            }
        }

        public IAlmacenSeccionRepository AlmacenSecciones
        {
            get
            {
                if (_almacensecciones == null)
                {
                    _almacensecciones = new AlmacenSeccionRepository(_repoContext);
                }
                return _almacensecciones;
            }
        }

        public IUnidadMedidaRepository UnidadesMedida
        {
            get
            {
                if (_unidadesmedida == null)
                {
                    _unidadesmedida = new UnidadMedidaRepository(_repoContext);
                }
                return _unidadesmedida;
            }
        }

        public IConversionRepository Conversiones
        {
            get
            {
                if (_conversiones == null)
                {
                    _conversiones = new ConversionRepository(_repoContext);
                }
                return _conversiones;
            }
        }

        public IArticuloRepository Articulos 
        {
            get
            {
                if (_articulos == null)
                {
                    _articulos = new ArticuloRepository(_repoContext);
                }
                return _articulos;
            }
        }

        public ISolicitudRepository Solicitudes
        {
            get
            {
                if (_solicitudes == null)
                {
                    _solicitudes = new SolicitudRepository(_repoContext);
                }
                return _solicitudes;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
        
        public void Save()
        {
            _repoContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}

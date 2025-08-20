using Domain.Catalogos.Commands;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL.Extensions;

namespace Infra.Catalogos.Commands
{
    public class CatalogoChange : ICatalogoChange
    {
        private readonly CatalogoDBContext _context;
        public CatalogoChange(CatalogoDBContext context)
        {
            _context = context;
        }
        public async Task<Catalogo> Change(CatalogoChangeCmd cmd)
        {

            ////var tt = await this._context.Catalogo.FirstOrDefaultAsync(x => x.id == cmd.itemCatalogo.id);

            ////if (tt != null)
            ////{ 
            ////    tt.name = cmd.itemCatalogo.name;
            ////    tt.description = cmd.itemCatalogo.description;
            ////    tt.alter_at = cmd.itemCatalogo.alter_at;

            ////    await _context.SaveChangesAsync();


            ////    //await _context.Catalogo.ExecuteUpdateAsync(cnf => 
            ////    //cnf.SetProperty(f => f.name, tt.name)
            ////    //.SetProperty(f => f.description, tt.description)
            ////    //.SetProperty(f => f.alter_at, tt.alter_at));
            ////}


            ////var item = cmd.itemCatalogo;

            //var t = await this._context.Catalogo.Where(x => x.id == cmd.itemCatalogo.id).AsNoTracking()
            //                                          .ExecuteUpdateAsync(cnf =>
            //                                                cnf.SetProperty(f => f.name, cmd.itemCatalogo.name)
            //                                                   .SetProperty(f => f.description, cmd.itemCatalogo.description)
            //                                                   .SetProperty(f => f.alter_at, cmd.itemCatalogo.alter_at)
            //                                          );

            ////var result = await this._context.Catalogo.AsNoTracking().FirstOrDefaultAsync(x => x.id == cmd.itemCatalogo.id);

            //return cmd.itemCatalogo;


            var sql = "UPDATE catalogo SET name = {0}, description = {1} WHERE id = {2}";

            _context.Database.ExecuteSqlRaw(
                sql,
                cmd.itemCatalogo.name,
                cmd.itemCatalogo.description,
                cmd.itemCatalogo.id);

            return cmd.itemCatalogo;


            //using (var connection = _context.Database.GetDbConnection())
            //{
            //    await connection.OpenAsync();
            //    using (var command = connection.CreateCommand())
            //    {
            //        command.CommandText = "UPDATE catalogo SET name = @name, description = @description, alter_at = @alter_at WHERE id = @id";

            //        var nameParam = command.CreateParameter();
            //        nameParam.ParameterName = "@name";
            //        nameParam.Value = cmd.itemCatalogo.name;
            //        command.Parameters.Add(nameParam);

            //        var descParam = command.CreateParameter();
            //        descParam.ParameterName = "@description";
            //        descParam.Value = cmd.itemCatalogo.description;
            //        command.Parameters.Add(descParam);

            //        var alterAtParam = command.CreateParameter();
            //        alterAtParam.ParameterName = "@alter_at";
            //        alterAtParam.Value = DateTime.Now;
            //        command.Parameters.Add(alterAtParam);

            //        var idParam = command.CreateParameter();
            //        idParam.ParameterName = "@id";
            //        idParam.Value = cmd.itemCatalogo.id;
            //        command.Parameters.Add(idParam);

            //        await command.ExecuteNonQueryAsync();
            //    }
            //}

            //// Atualiza o objeto de retorno
            //cmd.itemCatalogo.alter_at = DateTime.Now;
            //return cmd.itemCatalogo;
        }
    }
}

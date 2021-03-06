﻿using System;
using System.Threading.Tasks;
using GreatWall.Data;
using GreatWall.Domain.Models;
using GreatWall.Domain.Repositories;
using GreatWall.Service.Abstractions;
using GreatWall.Service.Dtos;
using GreatWall.Service.Dtos.Extensions;
using Util;
using Util.Applications;
using Util.Exceptions;

namespace GreatWall.Service.Implements {
    /// <summary>
    /// 应用程序服务
    /// </summary>
    public class ApplicationService : ServiceBase, IApplicationService {
        /// <summary>
        /// 初始化应用程序服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="applicationRepository">应用程序仓储</param>
        public ApplicationService( IGreatWallUnitOfWork unitOfWork, IApplicationRepository applicationRepository ) {
            UnitOfWork = unitOfWork;
            ApplicationRepository = applicationRepository;
        }

        /// <summary>
        /// 工作单元
        /// </summary>
        public IGreatWallUnitOfWork UnitOfWork { get; set; }
        /// <summary>
        /// 应用程序仓储
        /// </summary>
        public IApplicationRepository ApplicationRepository { get; set; }

        /// <summary>
        /// 创建应用程序
        /// </summary>
        /// <param name="dto">应用程序参数</param>
        public async Task<Guid> CreateAsync( ApplicationDto dto ) {
            var entity = dto.ToEntity();
            await ValidateCreateAsync( entity );
            entity.Init();
            await ApplicationRepository.AddAsync( entity );
            await UnitOfWork.CommitAsync();
            return entity.Id;
        }

        /// <summary>
        /// 验证创建应用程序
        /// </summary>
        private async Task ValidateCreateAsync( Application entity ) {
            entity.CheckNull( nameof( entity ) );
            if( await ApplicationRepository.CanCreateAsync( entity ) == false )
                ThrowCodeRepeatException( entity );
        }

        /// <summary>
        /// 抛出编码重复异常
        /// </summary>
        private void ThrowCodeRepeatException( Application entity ) {
            throw new Warning( string.Format( GreatWallResource.DuplicateApplicationCode, entity.Code ) );
        }

        /// <summary>
        /// 修改应用程序
        /// </summary>
        /// <param name="dto">应用程序参数</param>
        public async Task UpdateAsync( ApplicationDto dto ) {
            var entity = dto.ToEntity();
            await ValidateUpdateAsync( entity );
            await ApplicationRepository.UpdateAsync( entity );
            await UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// 验证修改应用程序
        /// </summary>
        private async Task ValidateUpdateAsync( Application entity ) {
            entity.CheckNull( nameof( entity ) );
            if( await ApplicationRepository.CanUpdateAsync( entity ) == false )
                ThrowCodeRepeatException( entity );
        }

        /// <summary>
        /// 删除应用程序
        /// </summary>
        /// <param name="ids">用逗号分隔的Id列表，范例："1,2"</param>
        public async Task DeleteAsync( string ids ) {
            if( string.IsNullOrWhiteSpace( ids ) )
                return;
            var entities = await ApplicationRepository.FindByIdsAsync( ids );
            if( entities?.Count == 0 )
                return;
            await ApplicationRepository.RemoveAsync( entities );
            await UnitOfWork.CommitAsync();
        }
    }
}

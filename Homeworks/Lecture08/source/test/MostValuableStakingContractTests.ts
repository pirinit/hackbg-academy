import { expect } from "chai";
import { ethers } from "hardhat";
import { SignerWithAddress } from "@nomiclabs/hardhat-ethers/signers";

import { MostValuableToken, MostValuableRewardToken, MostValuableStakingContract } from "../typechain";
import { constants } from "../constants";

describe("MostValuableToken", function () {

    let mvt: MostValuableToken;
    let mvrt: MostValuableRewardToken;
    let mvsc: MostValuableStakingContract;

    let admin: SignerWithAddress;
    let addr1: SignerWithAddress;
    let addr2: SignerWithAddress;
    
    beforeEach(async () => {
        const MostValuableToken = await ethers.getContractFactory("MostValuableToken");
        mvt = await MostValuableToken.deploy(constants.MostValuableToken.TOKEN_SUPPLY);
        await mvt.deployed();

        const MostValuableRewardToken = await ethers.getContractFactory("MostValuableRewardToken");
        mvrt = await MostValuableRewardToken.deploy();
        await mvrt.deployed();

        const MostValuableStakingContract = await ethers.getContractFactory("MostValuableStakingContract");
        mvsc = await MostValuableStakingContract.deploy(mvt.address, mvrt.address);
        await mvsc.deployed();

        await mvrt.transferOwnership(mvsc.address);

        [admin, addr1, addr2] = await ethers.getSigners();
    })
  
    it("Should return proper staling and reward token addresses", async function () {
      expect(await mvsc.StakingTokenAddress()).to.equal(mvt.address);
      expect(await mvsc.RewardTokenAddress()).to.equal(mvrt.address);
      
    });

});
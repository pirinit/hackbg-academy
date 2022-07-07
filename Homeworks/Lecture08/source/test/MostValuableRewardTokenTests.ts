import { expect } from "chai";
import { ethers } from "hardhat";
import { SignerWithAddress } from "@nomiclabs/hardhat-ethers/signers";

import { MostValuableRewardToken } from "../typechain";
import { constants } from "../constants";

describe("MostValuableRewardToken", function () {

    let mvrt: MostValuableRewardToken;
    let admin: SignerWithAddress;
    let addr1: SignerWithAddress;
    let addr2: SignerWithAddress;
    
    
    beforeEach(async () => {
        const MostValuableRewardToken = await ethers.getContractFactory("MostValuableRewardToken");
        mvrt = await MostValuableRewardToken.deploy();
        await mvrt.deployed();

        [admin, addr1, addr2] = await ethers.getSigners();
    })
  
    it("Should return the proper token name and symbol", async function () {
      expect(await mvrt.symbol())
      .to.equal(constants.MostValuableRewardToken.TOKEN_SYMBOL);

      expect(await mvrt.name())
      .to.equal(constants.MostValuableRewardToken.TOKEN_NAME);
    });
  
    it("Owner should mint and transfer new tokens to provided address. ", async function () {
      const newTokens = 100;

      await mvrt.mint(newTokens, addr1.address);
      expect(await mvrt.totalSupply())
      .to.equal(newTokens);

      expect(await mvrt.balanceOf(addr1.address))
      .to.equal(newTokens);
    });

    it("Non owners trying to mint should revert", async function () {

      await expect(
        mvrt.connect(addr1).mint(123,addr1.address)
      ).to.be.revertedWith("Ownable: caller is not the owner");
    });

    it("Owner should transfer ownership", async function () {
      expect(await mvrt.owner())
      .to.equal(admin.address);
      
      await mvrt.transferOwnership(addr1.address);

      expect(await mvrt.owner())
      .to.equal(addr1.address);
    });
});